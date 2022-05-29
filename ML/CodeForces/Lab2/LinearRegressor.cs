using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lab2_Extra
{
    public static class Extensions
    {
        private static readonly Random Rand = new Random();

        public static void Swap<T>(this IList<T> arr, int first, int second)
        {
            var temp = arr[first];
            arr[first] = arr[second];
            arr[second] = temp;
        }

        public static void Shuffle<T>(this IList<T> arr)
        {
            for (int i = 0; i < arr.Count - 1; i++)
                arr.Swap(i, Rand.Next(i + 1, arr.Count - 1));
        }

        public static void Distribution(this IList<double> arr, int featuresCount)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                arr[i] = 1.0 / Rand.Next(2 * featuresCount, int.MaxValue);

                if (Rand.Next(1) == 1)
                    arr[i] *= -1;
            }
        }

        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            foreach (var el in list)
                action(el);
        }

        public static Vector Sum(this IEnumerable<Vector> vectors, int vectorSize)
        {
            var sum = Vector.GetNullVector(vectorSize);

            return vectors.Aggregate(sum, (current, vec) => current + vec);
        }
    }

    public class Vector : IEnumerable<double>
    {
        private readonly IList<double> _coords;

        public Vector(IList<double> coords)
        {
            _coords = coords;
        }

        #region Operators

        public static Vector operator +(Vector first, Vector second)
            => new Vector(first._coords.Zip(second._coords, (x, y) => x + y).ToArray());
        public static Vector operator +(Vector vec, double num)
            => new Vector(vec._coords.Select(c => c + num).ToArray());
        public static Vector operator -(Vector first, Vector second)
            => new Vector(first._coords.Zip(second._coords, (x, y) => x - y).ToArray());
        public static Vector operator -(Vector vec, double num)
            => new Vector(vec._coords.Select(c => c - num).ToArray());
        public static Vector operator *(Vector first, Vector second)
            => new Vector(first._coords.Zip(second._coords, (x, y) => x * y).ToArray());
        public static Vector operator *(Vector vec, double num)
            => new Vector(vec._coords.Select(c => c * num).ToArray());
        public static Vector operator /(Vector first, Vector second)
            => new Vector(first._coords.Zip(second._coords, (x, y) => x / y).ToArray());
        public static Vector operator /(Vector vec, double num)
            => new Vector(vec._coords.Select(c => c / num).ToArray());

        #endregion

        public static Vector GetNullVector(int size)
            => new Vector(Enumerable.Repeat(0.0, size).ToArray());

        public bool Equals(Vector other)
            => !_coords.Where((c, i) => c.CompareTo(other._coords[i]) != 0).Any();
        public double Dot(Vector other)
            => _coords.Zip(other._coords, (x, y) => x * y).Sum();

        public double Avg() => _coords.Average();
        public double Deviation()
        {
            var avg = Avg();

            double sum = _coords.Sum(xi => (xi - avg) * (xi - avg));

            return Math.Sqrt(sum / _coords.Count);
        }
        public Vector Square()
            => new Vector(_coords.Select(c => c * c).ToArray());
        public Vector Sqrt()
            => new Vector(_coords.Select(Math.Sqrt).ToArray());

        public void Normalize(double[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
                if (vec[i].CompareTo(0) != 0)
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

        public IEnumerator<double> GetEnumerator()
        {
            return _coords.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Object
    {
        public Vector Features { get; set; }
        public double Label { get; set; }

        public Object(Vector features, int label)
        {
            Features = features;
            Label = label;
        }
    }

    public class LinearRegression
    {
        private readonly Stopwatch _timer = Stopwatch.StartNew();
        private const int TimeLimit = 1400;

        private readonly IList<Object> _dataSet;
        private Vector _weights;

        private readonly Func<double, double>[] _featuresNormalization;
        private Func<double, double> _labelsNormalization;

        public LinearRegression()
        {
            var query = Console.ReadLine().Split();

            var objectCount = int.Parse(query[0]);
            var featuresCount = int.Parse(query[1]);

            _dataSet = new Object[objectCount];
            for (int i = 0; i < objectCount; i++)
            {
                var objectData = Console.ReadLine().Split().Select(double.Parse).ToList();
                _dataSet[i] = new Object(new Vector(objectData.ToArray()), (int)objectData.Last());
                _dataSet[i].Features[featuresCount] = 1.0;
            }

            //_featuresNormalization = new Func<double, double>[featuresCount];
            //Normalize();

            _weights = GetInitialWeights();

            if (featuresCount == 1)
                OneFeature();
            else
                MoreFeatures();
        }
        private void Normalize()
        {
            var features = Enumerable.Range(0, _dataSet[0].Features.Size)
                .Select(i => new Vector(_dataSet.Select(obj => obj.Features[i]).ToArray()))
                .ToArray();

            //var featuresAvg = features.Select(f => f.Avg()).ToArray();
            //var featuresDeviation = features.Select(f => f.Deviation()).ToArray();

            //for (int i = 0; i < _featuresNormalization.Length; i++)
            //{
            //    int featureNum = i;

            //    _dataSet.ForEach(obj => obj.Features[featureNum] = (obj.Features[featureNum] - featuresAvg[featureNum]) / featuresDeviation[featureNum]);
            //    _featuresNormalization[featureNum] = f => f * featuresDeviation[featureNum] + featuresAvg[featureNum];
            //}

            var featuresMax = features.Select(f => f.Select(Math.Abs).Max()).ToArray();
            for (int i = 0; i < _featuresNormalization.Length; i++)
            {
                int featureNum = i;

                _dataSet.ForEach(obj => obj.Features[featureNum] /= featuresMax[featureNum]);
                _featuresNormalization[featureNum] = f => f * featuresMax[featureNum];
            }

            //var labels = new Vector(_dataSet.Select(obj => obj.Label).ToArray());

            //var labelsAvg = labels.Avg;
            //var labelsDeviation = labels.Deviation();

            //_dataSet.ForEach(obj => obj.Label = (obj.Label - labelsAvg) / labelsDeviation);
            //_labelsNormalization = l => l * labelsDeviation + labelsAvg;
        }
        private Vector GetInitialWeights()
        {
            var featuresCount = _dataSet[0].Features.Size;

            var weights = new double[featuresCount];
            weights.Distribution(featuresCount);

            return new Vector(weights);
        }

        private void OneFeature()
        {
            double featuresAvg = _dataSet.Average(obj => obj.Features[0]);
            double labelsAvg = _dataSet.Average(obj => obj.Label);

            double num = 0;
            double denom = 0;

            foreach (var obj in _dataSet)
            {
                denom += Math.Pow(obj.Features[0] - featuresAvg, 2);
                num += (obj.Features[0] - featuresAvg) * (obj.Label - labelsAvg);
            }

            _weights[0] = denom.CompareTo(0) == 0 ? 0 : num / denom;
            _weights[1] = labelsAvg - _weights[0] * featuresAvg;
        }
        private void MoreFeatures()
        {
            //const int batchSize = 1;
            var rand = new Random();
            //bool isRunning = true;

            while (_timer.ElapsedMilliseconds < TimeLimit)
            {
                for (int i = _dataSet.Count - 1; i > 0; i--)
                {
                    var objNum = rand.Next(0, i);
                    _weights -= CalculateStep(_dataSet[objNum]);
                    _dataSet.Swap(objNum, i);
                }

                _weights -= CalculateStep(_dataSet[0]);


                //foreach (var obj in _dataSet)
                //    _weights -= CalculateStep(obj);


                //_dataSet.Shuffle();

                //int objNum;
                //for (objNum = 0; objNum < _dataSet.Count - batchSize + 1; objNum += batchSize)
                //{
                //    _weights -= CalculateStep(batchSize, objNum);

                //    if (_timer.ElapsedMilliseconds > TimeLimit)
                //        isRunning = false;
                //}

                //if (objNum != _dataSet.Count - batchSize + 1) //TODO: need to do
                //{
                //    objNum -= batchSize;

                //    var objCount = _dataSet.Count - objNum;
                //    _weights -= CalculateStep(objCount, objNum);
                //}
            }
        }

        private Vector CalculateStep(int batchSize, int startIndex)
        {
            var diff = Vector.GetNullVector(batchSize);
            for (int i = 0; i < batchSize; i++)
                diff[i] = _dataSet[startIndex + i].Label - _weights.Dot(_dataSet[startIndex + i].Features);

            var grad = Vector.GetNullVector(_weights.Size);
            for (var i = 0; i < grad.Size; i++)
            {
                for (var j = 0; j < batchSize; j++)
                    grad[i] -= 2 * diff[j] * _dataSet[j + startIndex].Features[i];

                grad[i] /= batchSize;
            }

            var dots = Vector.GetNullVector(batchSize);
            for (int i = 0; i < batchSize; i++)
                dots[i] = grad.Dot(_dataSet[i + startIndex].Features);

            double dotSum = dots.Sum(dot => dot * dot);

            double gradStep = 0;
            if (dotSum.CompareTo(0) == 0)
                gradStep = 0;
            else
            {
                gradStep += diff.Dot(dots);
                gradStep = -gradStep / dotSum;
            }

            return grad * gradStep;
        }
        private Vector CalculateStep(Object obj)
        {
            var diff = obj.Label - _weights.Dot(obj.Features);

            var grad = new Vector(obj.Features.Select(f => -2 * diff * f).ToArray());

            var prediction = grad.Dot(obj.Features);
            var predSquare = prediction * prediction;

            double gradStep = 0;

            if (predSquare.CompareTo(0) != 0)
            {
                gradStep += diff * prediction;
                gradStep /= -predSquare;
            }

            return grad * gradStep;
        }

        public void Print()
        {
            //for (int i = 0; i < _featuresNormalization.Length; i++)
            //    Console.WriteLine(_featuresNormalization[i].Invoke(_weights[i]));

            //Console.WriteLine(_weights.Last());

            //Console.WriteLine(_labelsNormalization.Invoke(_weights.Last()));

            Console.WriteLine(string.Join("\n", _weights));
        }
    }

    public class AdamRegression
    {
        private readonly Random _rand = new Random();
        private readonly Stopwatch _timer = Stopwatch.StartNew();
        private const int TimeLimit = 1400;

        private readonly IList<Object> _dataSet;
        private readonly Vector _weights;

        private readonly Func<double, double> _lossFunc = margin => 2 * margin;

        private double _curBeta1 = Beta1, _curBeta2 = Beta2;
        private readonly double[] _m;
        private readonly double[] _v;

        private const double LearningRate = 1e-3;
        private const double Beta1 = 0.9, Beta2 = 0.999;
        private const double Epsilon = 1e-8;

        public AdamRegression()
        {
            var query = Console.ReadLine().Split();

            var objectCount = int.Parse(query[0]);
            var featuresCount = int.Parse(query[1]);

            _dataSet = new Object[objectCount];
            for (int i = 0; i < objectCount; i++)
            {
                var objectData = Console.ReadLine().Split().Select(double.Parse).ToList();
                _dataSet[i] = new Object(new Vector(objectData.ToArray()), (int)objectData.Last());
                _dataSet[i].Features[featuresCount] = 1.0;
            }

            _m = new double[featuresCount + 1];
            _v = new double[featuresCount + 1];

            _weights = GetInitialWeights();

            if (featuresCount == 1)
                OneFeature();
            else
                MoreFeatures();
        }
        private Vector GetInitialWeights()
        {
            var featuresCount = _dataSet[0].Features.Size;

            var weights = new double[featuresCount];
            weights.Distribution(featuresCount);

            return new Vector(weights);
        }

        private Object GetRandomObject() => _dataSet[_rand.Next(0, _dataSet.Count - 1)];

        private void OneFeature()
        {
            double featuresAvg = _dataSet.Average(obj => obj.Features[0]);
            double labelsAvg = _dataSet.Average(obj => obj.Label);

            double num = 0;
            double denom = 0;

            foreach (var obj in _dataSet)
            {
                denom += Math.Pow(obj.Features[0] - featuresAvg, 2);
                num += (obj.Features[0] - featuresAvg) * (obj.Label - labelsAvg);
            }

            _weights[0] = denom.CompareTo(0) == 0 ? 0 : num / denom;
            _weights[1] = labelsAvg - _weights[0] * featuresAvg;
        }
        private void MoreFeatures()
        {
            while (_timer.ElapsedMilliseconds < TimeLimit)
            {
                for (int i = _dataSet.Count - 1; i > 0; i--)
                {
                    var objNum = _rand.Next(0, i);
                    GradientStep(_dataSet[objNum]);
                    _dataSet.Swap(objNum, i);
                }

                GradientStep(_dataSet[0]);
            }
        }

        private void AdamStep(ref double a, ref double b, double lr = 1e-5, double beta1 = 0.9, double beta2 = 0.999, double epsilon = 1e-8)
        {
            double ma = 0, va = 0, mb = 0, vb = 0;

            int stepNum = 0;
            while (_timer.ElapsedMilliseconds < TimeLimit)
            {
                var curObj = GetRandomObject();
                var (gradA, gradB) = CalculateGradient(curObj, a, b);
                stepNum++;

                ma = beta1 * ma + (1 - beta1) * gradA;
                va = beta2 * va + (1 - beta2) * gradA * gradA;
                mb = beta1 * mb + (1 - beta1) * gradB;
                vb = beta2 * vb + (1 - beta2) * gradB * gradB;

                var momentMa = ma / (1 - Math.Pow(beta1, stepNum));
                var momentVa = va / (1 - Math.Pow(beta2, stepNum));
                var momentMb = mb / (1 - Math.Pow(beta1, stepNum));
                var momentVb = vb / (1 - Math.Pow(beta2, stepNum));

                a -= lr * momentMa / (Math.Sqrt(momentVa) + epsilon);
                b -= lr * momentMb / (Math.Sqrt(momentVb) + epsilon);
            }
        }
        private (double a, double b) CalculateGradient(Object obj, double a, double b)
        {
            var gradient = obj.Features * a + b - obj.Label;
            return ((obj.Features * gradient).Avg(), gradient.Avg());
        }


        private void GradientStep(Object curObj)
        {
            var gradient = CalculateGradient(curObj);

            for (int i = 0; i < gradient.Size; i++)
            {
                _m[i] = Beta1 * _m[i] + (1 - Beta1) * gradient[i];
                var m = _m[i] / (1 - _curBeta1);
                _v[i] = Beta2 * _v[i] + (1 - Beta2) * gradient[i] * gradient[i];
                var v = _v[i] / (1 - _curBeta2);

                _weights[i] -= LearningRate * m / (Math.Sqrt(v) + Epsilon);
            }

            _curBeta1 *= Beta1;
            _curBeta2 *= Beta2;
        }
        private Vector CalculateGradient(Object obj)
        {
            var forecast = obj.Features.Dot(_weights);
            var margin = forecast - obj.Label;

            return obj.Features * _lossFunc(margin);
        }

        public void Print()
        {
            Console.WriteLine(string.Join("\n", _weights));
        }
    }

    public class EntryPoint
    {
        public static void Main()
        {
            var comp = new AdamRegression();
            comp.Print();

            Console.ReadLine();
        }
    }
}
