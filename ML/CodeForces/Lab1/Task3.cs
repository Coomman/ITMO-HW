using System;
using System.Linq;
using System.Collections.Generic;

namespace Lab1
{
    internal class Vector
    {
        private readonly IList<int> _coords;

        public Vector(IList<int> coords)
        {
            _coords = coords;
        }

        public double ManhattanDistance(Vector other)
            => _coords.Zip(other._coords, (x, y) => Math.Abs(x - y)).Sum();
        public double EuclideanDistance(Vector other)
            => Math.Sqrt(_coords.Zip(other._coords, (x, y) => Math.Pow(x - y, 2)).Sum());
        public double ChebyshevDistance(Vector other)
            => _coords.Zip(other._coords, (x, y) => Math.Abs(x - y)).Max();

        public bool Equals(Vector other)
            => !_coords.Where((coord, i) => coord.CompareTo(other._coords[i]) != 0).Any();

        public int this[int index] => _coords[index];
    }

    internal class Object
    {
        public Vector Features { get; }
        public int Label { get; }

        public double Distance { get; set; }

        public Object(Vector features, int label = 0)
        {
            Features = features;
            Label = label;
        }
    }

    internal class RegressionComputer
    {
        private readonly Func<Vector, Vector, double> _distanceFunc;
        private readonly Func<double, double> _kernelFunc;

        private double _windowWidth;

        public RegressionComputer(string distanceFunc, string kernelFunc)
        {
            switch (distanceFunc[0])
            {
                case 'm':
                    _distanceFunc = ManhattanDistance;
                    break;
                case 'e':
                    _distanceFunc = EuclideanDistance;
                    break;
                case 'c':
                    _distanceFunc = ChebyshevDistance;
                    break;
            }

            switch (kernelFunc)
            {
                case "uniform":
                    _kernelFunc = UniformFunc;
                    break;
                case "triangular":
                    _kernelFunc = Triangular;
                    break;
                case "epanechnikov":
                    _kernelFunc = EpanechnikovFunc;
                    break;
                case "quartic":
                    _kernelFunc = QuarticFunc;
                    break;
                case "triweight":
                    _kernelFunc = TriweightFunc;
                    break;
                case "tricube":
                    _kernelFunc = TricubeFunc;
                    break;
                case "gaussian":
                    _kernelFunc = GaussianFunc;
                    break;
                case "cosine":
                    _kernelFunc = CosineFunc;
                    break;
                case "logistic":
                    _kernelFunc = LogisticFunc;
                    break;
                case "sigmoid":
                    _kernelFunc = SigmoidFunc;
                    break;
            }
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
        private static double Triangular(double u)
            => Math.Abs(u) >= 1 ? 0 : 1 - Math.Abs(u);
        private static double EpanechnikovFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : 0.75 * (1 - u * u);
        private static double CosineFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : Math.PI / 4 * Math.Cos(Math.PI / 2 * u);
        private static double QuarticFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : 0.9375 * Math.Pow(1 - u * u, 2);
        private static double TriweightFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : 1.09375 * Math.Pow(1 - u * u, 3);
        private static double TricubeFunc(double u)
            => Math.Abs(u) >= 1 ? 0 : 70.0 / 81 * Math.Pow(1 - Math.Abs(u * u * u), 3);

        private static double GaussianFunc(double u)
            => Math.Exp(-0.5 * u * u) / Math.Sqrt(2 * Math.PI);
        private static double LogisticFunc(double u)
            => 1 / (Math.Exp(u) + Math.Exp(-u) + 2);
        private static double SigmoidFunc(double u)
            => 2 / Math.PI / (Math.Exp(u) + Math.Exp(-u));

        #endregion

        public double Compute(IList<Object> dataSet, Object target, string windowType, int windowParam)
        {
            CalculateDistances(ref dataSet, target);

            if (windowType[0] == 'v')
                SetWindowWidth(dataSet, windowParam);
            else
                _windowWidth = windowParam;

            var averageValue = dataSet.Average(model => model.Label);
            if (_windowWidth.CompareTo(0) == 0)
            {
                var matchingModels = GetMatchingModels(dataSet, target);

                return matchingModels.Any() 
                    ? matchingModels.Average(model => model.Label) 
                    : averageValue;
            }

            var factors = CalculateFactors(dataSet);
            var factorsSum = factors.Sum();

            if (factorsSum.CompareTo(0) == 0)
                return averageValue;

            return dataSet.Select((obj, i) => factors[i] * obj.Label).Sum() / factorsSum;
        }
        private void SetWindowWidth(IList<Object> dataSet, int neighborNum)
        {
            _windowWidth = dataSet[neighborNum].Distance;
        }
        private static List<Object> GetMatchingModels(IList<Object> dataSet, Object target)
        {
            return dataSet
                .Where(model => model.Features.Equals(target.Features))
                .ToList();
        }

        private void CalculateDistances(ref IList<Object> dataSet, Object target)
        {
            foreach (var model in dataSet)
                model.Distance = _distanceFunc(model.Features, target.Features);

            dataSet = dataSet.OrderBy(model => model.Distance).ToList();
        }
        private double[] CalculateFactors(IList<Object> dataSet)
        {
            var factors = new double[dataSet.Count];

            for (int i = 0; i < dataSet.Count; i++)
                factors[i] = _kernelFunc(dataSet[i].Distance / _windowWidth);

            return factors;
        }
    }

    internal class Task3
    {
        public static void Main_()
        {
            var query = Console.ReadLine().Split();

            var dataSetSize = int.Parse(query[0]);
            var attributesCount = int.Parse(query[1]);

            var dataSet = new Object[dataSetSize];
            for (int i = 0; i < dataSetSize; i++)
            {
                var objData = Console.ReadLine().Split().Select(int.Parse).ToArray();
                dataSet[i] = new Object(new Vector(objData.Take(attributesCount).ToArray()), objData.Last());
            }
            var target = new Object(new Vector(Console.ReadLine().Split().Select(int.Parse).ToArray()));

            var computer = new RegressionComputer(Console.ReadLine(), Console.ReadLine());
            var result = computer.Compute(dataSet, target, Console.ReadLine(), int.Parse(Console.ReadLine()));

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
