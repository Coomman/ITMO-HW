using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab2
{
    public static class Extensions
    {
        public static Vector Sum(this IEnumerable<Vector> list)
        {
            var vectors = list.ToList();

            var sum = Vector.GetNullVector(vectors.First().Size);

            return vectors.Aggregate(sum, (current, vec) => current + vec);
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var el in list)
                action(el);
        }
    }

    public class Vector
    {
        private readonly IList<double> _coords;

        public Vector(IList<double> coords)
        {
            _coords = coords;
        }

        public static Vector operator +(Vector first, Vector second)
            => new Vector(first._coords.Zip(second._coords, (x, y) => x + y).ToArray());
        public static Vector operator -(Vector first, Vector second)
            => new Vector(first._coords.Zip(second._coords, (x, y) => x - y).ToArray());
        public static Vector operator *(Vector vec, double num)
            => new Vector(vec._coords.Select(c => c * num).ToArray());

        public static Vector GetNullVector(int size)
            => new Vector(Enumerable.Repeat(0.0, size).ToArray());

        public bool Equals(Vector other)
            => !_coords.Where((coord, i) => coord.CompareTo(other._coords[i]) != 0).Any();
        public double Scalar(Vector other)
            => _coords.Zip(other._coords, (x, y) => x * y).Sum();
        public void Normalize(double[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
                if(vec[i].CompareTo(0) != 0)
                    _coords[i] /= vec[i];
        }
        public void Denormalize(double[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
                if (vec[i].CompareTo(0) != 0)
                    _coords[i] *= vec[i];
        }

        public double this[int index]
        {
            get => _coords[index];
            set => _coords[index] = value;
        }
        public int Size => _coords.Count;

        public override string ToString()
            => string.Join("\n", _coords);
    }

    public class Object
    {
        public Vector Features { get; }
        public double Label { get; set; }

        public Object(Vector features, int label)
        {
            Features = features;
            Label = label;
        }
    }

    public class LinearRegressor
    {
        private readonly double _gradientStep = Math.Pow(10, -10);

        private const double RequiredPrecisionPercentage = 1;
        private const double EMA = 0.76;
        private const int BatchSize = 5;

        private readonly IList<Object> _dataSet;

        private Vector _weights;
        private int _featuresCount;
        private double[] _normalizationVector;
        private double _labelFactor;

        #region LossFunc

        private static double Derivative(double margin)
            => 2 * margin;

        #endregion

        #region GradientFunc

        private void GradientDescent(Object obj)
        {
            var forecast = obj.Features.Scalar(_weights);
            var margin = forecast - obj.Label;

            var gradientVector = obj.Features * Derivative(margin) * _gradientStep;

            _weights -= gradientVector;
        }

        #endregion

        public LinearRegressor(IList<Object> dataSet)
        {
            _dataSet = dataSet;
        }

        public Vector Compute()
        {
            _featuresCount = _dataSet.First().Features.Size;
            SetInitialWeights();
            Normalize();

            var timer = Stopwatch.StartNew();

            int i = 0;
            //while (GetErrorPercentage() > RequiredPrecisionPercentage)
            while (timer.ElapsedMilliseconds < 1000)
            {
                GradientDescent(_dataSet[i]);
                i = (i + 1) % _dataSet.Count;
            }

            _weights.Denormalize(_normalizationVector);

            return _weights;
        }

        private void SetInitialWeights()
        {
            var weights = new double[_featuresCount];
            var random = new Random();

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = 1.0 / random.Next(2 * _featuresCount, int.MaxValue);

                if (random.Next(1) == 1)
                    weights[i] *= -1;
            }

            _weights = new Vector(weights);
        }

        private void Normalize()
        {
            _normalizationVector =  Enumerable.Range(0, _featuresCount - 1)
                .Select((_, i) => Math.Abs(Max(i) + Min(i)))
                .ToArray();

            _dataSet
                .ForEach(obj => obj.Features.Normalize(_normalizationVector));

            _labelFactor = Math.Abs(_dataSet.Max(obj => obj.Label) + _dataSet.Min(obj => obj.Label));

            _dataSet
                .ForEach(obj => obj.Label /= _labelFactor.CompareTo(0) == 0 ? 1 :_labelFactor);
        }

        private double Max(int i)
            => _dataSet.Max(obj => obj.Features[i]);
        private double Min(int i)
            => _dataSet.Min(obj => obj.Features[i]);

        private double GetErrorPercentage()
        {
            var error = 0.0;

            foreach (var obj in _dataSet)
            {
                var forecast = _weights.Scalar(obj.Features);

                error += Math.Abs(forecast - obj.Label) / (Math.Abs(forecast) + Math.Abs(obj.Label));
            }

            return error * 100 / _dataSet.Count;
        }
    }

    public class Task1
    {
        public static void Main_()
        {
            var query = Console.ReadLine().Split();

            var objectCount = int.Parse(query[0]);
            var featuresCount = int.Parse(query[1]);

            var dataSet = new Object[objectCount];
            for (int i = 0; i < objectCount; i++)
            {
                var objectData = Console.ReadLine().Split().Select(double.Parse).ToList();
                dataSet[i] = new Object(new Vector(objectData.ToArray()), (int) objectData.Last());
                dataSet[i].Features[featuresCount] = -1.0;
            }

            var comp = new LinearRegressor(dataSet);
            var result = comp.Compute();

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
