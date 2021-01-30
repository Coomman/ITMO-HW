using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;

using RegressionComputer.Models;

namespace RegressionComputer
{
    public enum DistanceFunc {Manhattan, Euclidean, Chebyshev}
    public enum KernelFunc { Uniform, Triangular, Epanechnikov, Quartic, Triweight, Tricube, Cosine, Gaussian, Logistic, Sigmoid }

    public class Regressor
    {
        private readonly Dictionary<DistanceFunc, Func<Vector, Vector, double>> _distanceDict;
        private readonly Dictionary<KernelFunc, Func<double, double>> _kernelDict;

        public Regressor()
        {
            _distanceDict = new Dictionary<DistanceFunc, Func<Vector, Vector, double>>
            {
                [DistanceFunc.Manhattan] = ManhattanDistance,
                [DistanceFunc.Euclidean] = EuclideanDistance,
                [DistanceFunc.Chebyshev] = ChebyshevDistance
            };

            _kernelDict = new Dictionary<KernelFunc, Func<double, double>>
            {
                [KernelFunc.Uniform] = UniformFunc, 
                [KernelFunc.Triangular] = TriangularFunc,
                [KernelFunc.Epanechnikov] = EpanechnikovFunc,
                [KernelFunc.Quartic] = QuarticFunc,
                [KernelFunc.Triweight] = TriweightFunc,
                [KernelFunc.Tricube] = TricubeFunc,
                [KernelFunc.Gaussian] = GaussianFunc,
                [KernelFunc.Cosine] = CosineFunc,
                [KernelFunc.Logistic] = LogisticFunc,
                [KernelFunc.Sigmoid] = SigmoidFunc
            };
        }

        #region Distance

        private static double ManhattanDistance(Vector x, Vector y)
            => x.ManhattanDistance(y);
        private static double EuclideanDistance(Vector x, Vector y)
            => x.EuclideanDistance(y);
        private static double ChebyshevDistance(Vector x, Vector y)
            => x.ChebyshevDistance(y);

        #endregion

        #region KernelFuncs

        private static double UniformFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : 0.5;
        private static double TriangularFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : 1 - Math.Abs(u);
        private static double EpanechnikovFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : 0.75 * (1 - u * u);
        private static double QuarticFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : 0.9375 * Math.Pow(1 - u * u, 2);
        private static double TriweightFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : 1.09375 * Math.Pow(1 - u * u, 3);
        private static double TricubeFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : 70.0 / 81 * Math.Pow(1 - Math.Abs(u * u * u), 3);
        private static double CosineFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : Math.PI / 4 * Math.Cos(Math.PI / 2 * u);
        private static double GaussianFunc(double u)
            => Math.Exp(-0.5 * u * u) / Math.Sqrt(2 * Math.PI);
        private static double LogisticFunc(double u)
            => 1 / (Math.Exp(u) + Math.Exp(-u) + 2);
        private static double SigmoidFunc(double u)
            => 2 / Math.PI / (Math.Exp(u) + Math.Exp(-u));

        #endregion

        public FinalResult Compute(IList<IObject> objects, int classCount)
        {
            var timer = Stopwatch.StartNew();

            CalculateDistances(objects);

            var validations = ProcessValidation(objects, classCount);

            var bestResult = GetBestResult(validations);

            return new FinalResult
            {
                DistanceFunc = bestResult.DistanceFunc.ToString(),
                KernelFunc = bestResult.KernelFunc.ToString(),
                ValidationCount = validations.Count,
                F1Score = GetF1Scores(validations, bestResult),
                ElapsedTime = $"Done in: {timer.Elapsed.TotalSeconds} seconds"
            };
        }

        private static void CalculateDistances(IList<IObject> objects)
        {
            objects
                .AsParallel()
                .ForAll(obj => CalculateDistances(objects.Without(obj).ToList(), obj));

            void CalculateDistances(IList<IObject> neighbors, IObject target)
            {
                target.Distances = new Dictionary<DistanceFunc, double[]>
                {
                    [DistanceFunc.Manhattan] = neighbors.Select(n => ManhattanDistance(target.Features, n.Features)).ToArray(),
                    [DistanceFunc.Euclidean] = neighbors.Select(n => EuclideanDistance(target.Features, n.Features)).ToArray(),
                    [DistanceFunc.Chebyshev] = neighbors.Select(n => ChebyshevDistance(target.Features, n.Features)).ToArray()
                };
            }
        }
        private ConcurrentBag<ValidationResult> ProcessValidation(IList<IObject> objects, int classCount)
        {
            var validations = new ConcurrentBag<ValidationResult>();

            _distanceDict.Keys
                .ToList()
                .ForEach(dist => _kernelDict.Keys
                    .ToList()
                    .ForEach(kernel => objects
                        .AsParallel()
                        .Select((_, i) => LeaveOneOut(objects, classCount, dist, kernel, i - 1))
                        .ForAll(validations.Add)));

            return validations;
        }
        private static ValidationResult GetBestResult(ConcurrentBag<ValidationResult> validations)
        {
            return validations
                .AsParallel()
                .OrderByDescending(res => res.F1Score)
                .First();
        }
        private static double[] GetF1Scores(ConcurrentBag<ValidationResult> validations, ValidationResult bestResult)
        {
            return validations
                .AsParallel()
                .Where(res => res.DistanceFunc == bestResult.DistanceFunc && res.KernelFunc == bestResult.KernelFunc)
                .OrderBy(res => res.NeighborCount)
                .Select(res => res.F1Score)
                .ToArray();
        }

        private ValidationResult LeaveOneOut(IList<IObject> objects, int classCount, DistanceFunc distFunc, KernelFunc kernelFunc, int neighborCount)
        {
            var matrix = new int[classCount][];
            for (int i = 0; i < classCount; i++)
                matrix[i] = new int[classCount];

            objects
                .AsParallel()
                .ForAll(obj => Validate(new DataSet(objects.Without(obj).ToList(), obj, distFunc, _kernelDict[kernelFunc]), neighborCount, matrix));

            var confMatrix = new ConfusionMatrix(matrix);

            return new ValidationResult
            {
                DistanceFunc = distFunc,
                KernelFunc = kernelFunc,
                NeighborCount = neighborCount + 1,
                F1Score = (confMatrix.MicroF1Score() + confMatrix.MacroF1Score()) / 2
            };
        }
        private static void Validate(DataSet dataSet, int neighborCount, int[][] matrix)
        {
            var result = dataSet.GetTargetLabel(neighborCount);
            var prediction = Math.Max((int) Math.Round(result), 0);

            lock (matrix.SyncRoot)
                matrix[dataSet.Target.Class][prediction]++;
        }
    }
}
