using System;
using System.Linq;
using System.Collections.Generic;

using RegressionComputer.Models;

namespace RegressionComputer
{
    internal class DataSet
    {
        private readonly DistanceFunc _distanceFunc;
        private readonly Func<double, double> _kernelFunc;
        private double _windowWidth;

        private readonly IList<IObject> _objects;
        public IObject Target { get; }

        public DataSet(IList<IObject> objects, IObject target, DistanceFunc distanceFunc, Func<double, double> kernelFunc)
        {
            _objects = objects;
            Target = target;
            _distanceFunc = distanceFunc;
            _kernelFunc = kernelFunc;
        }

        public double GetTargetLabel(int neighborCount)
        {
            SetWindowWidth(neighborCount);

            if (_windowWidth.CompareTo(0) == 0)
                return GetAverage();

            var weights = GetWeights();
            var weightsSum = weights.Sum();

            if (weightsSum.CompareTo(0) == 0)
                return _objects.Average(obj => obj.Class);

            var result = _objects.Zip(weights, (obj, weight) => obj.Labels * weight).Sum() / weightsSum;

            if (result.Size == 1)
                return result[0];

            return result.ArgMax;
        }

        private void SetWindowWidth(int neighborCount)
        {
            _windowWidth = neighborCount == -1
                ? 0
                : Target.Distances[_distanceFunc].OrderBy(d => d).ElementAt(neighborCount);
        }
        private double[] GetWeights()
        {
            return Target.Distances[_distanceFunc].Select(dist=> _kernelFunc(dist / _windowWidth)).ToArray();
        }

        private double GetAverage()
        {
            var matchingObjects = GetMatchingObjects();

            return matchingObjects.Any()
                ? matchingObjects.Average(obj => obj.Class)
                : _objects.Average(obj => obj.Class);
        }
        private IList<IObject> GetMatchingObjects()
        {
            return _objects
                .Where(obj => obj.Features.Equals(Target.Features))
                .ToList();
        }
    }
}
