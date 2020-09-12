using System;
using System.Linq;
using System.Collections.Generic;

using Lab1_1.DTO;

namespace Lab1_1
{
    public class MultiMinimumFinder
    {
        private readonly List<long> _fibonacci = new List<long> { 1, 1 };

        private Func<Vector, double> _func;
        private readonly Func<MultiSegment, MultiSegment> _method;
        private Vector _direction;

        private MultiSegment _initialSegment;

        private readonly double _error = Math.Pow(10, -5);
        private int _iterationCount;

        public MultiMinimumFinder(Methods method)
        {
            _method = method switch
            {
                Methods.Dichotomy => DichotomyMethod,
                Methods.GoldenRatio => GoldenRatioMethod,
                Methods.Fibonacci => FibonacciMethod
            };
        }

        private void GenerateFibonacci(long rightBorder)
        {
            while (_fibonacci.Last() <= rightBorder)
                _fibonacci.Add(_fibonacci[^1] + _fibonacci[^2]);
        }

        public (MultiSegment segment, int iterationCount) GetMinimum(MultiInitialData data)
        {
            _func = data.Func;
            _iterationCount = 0;
            _direction = (data.To - data.From).Normalized;

            if (_method == FibonacciMethod)
                GenerateFibonacci((long)((data.To - data.From).Length / _error));

            var segment = new MultiSegment { From = data.From, To = data.To };
            _initialSegment = segment;
            while (segment.Length >= _error)
            {
                _iterationCount++;
                segment = _method(segment);
            }

            return (segment, _iterationCount);
        }

        private MultiSegment DichotomyMethod(MultiSegment segment)
        {
            var delta = _error / 2 * 0.99;

            var x1 = segment.Mid - _direction * delta;
            var x2 = segment.Mid + _direction * delta;

            return GetNextSegment(segment, x1, x2);
        }
        private MultiSegment GoldenRatioMethod(MultiSegment segment)
        {
            var x1 = segment.From + (segment.To - segment.From) * 0.381966011;
            var x2 = segment.From + (segment.To - segment.From) * 0.618003399;

            return GetNextSegment(segment, x1, x2);
        }
        private MultiSegment FibonacciMethod(MultiSegment segment)
        {
            var factor = (_initialSegment.To - _initialSegment.From).Length / _fibonacci.Last();

            var x1 = segment.From + _direction * _fibonacci[^(_iterationCount + 2)] * factor;
            var x2 = segment.From + _direction * _fibonacci[^(_iterationCount + 1)] * factor;

            return GetNextSegment(segment, x1, x2);
        }

        private MultiSegment GetNextSegment(MultiSegment segment, Vector x1, Vector x2)
        {
            var res1 = _func(x1);
            var res2 = _func(x2);

            if (res1.CompareTo(res2) == 0)
                return new MultiSegment { From = x1, To = x2 };

            return res1.CompareTo(res2) < 0
                ? new MultiSegment { From = segment.From, To = x2 }
                : new MultiSegment { From = x1, To = segment.To };
        }

        public MultiSegment GetMultiSegmentWithMinimum(Func<Vector, double> func, Vector dir)
        {
            _func = func;
            var startVector = Vector.GetZeroVector(dir.Size);

            var (step, segment) = GetInitialMultiSegment(startVector, dir);

            return GetMultiSegmentWithMinimum(segment, step, dir);
        }

        private (double step, MultiSegment segment) GetInitialMultiSegment(Vector startVector, Vector dir)
        {
            var segment = new MultiSegment { From = startVector };
            double step = _error;

            var forwardVector = startVector + dir * _error;
            var backVector = startVector - dir * _error;

            if (_func(forwardVector) < _func(startVector))
            {
                segment.To = forwardVector;
            }
            else if (_func(startVector - dir * _error) < _func(startVector))
            {
                segment.To = backVector;
                step = -_error;
            }
            else
            {
                segment.From = forwardVector;
                segment.To = backVector;
            }

            return (step, segment);
        }
        private MultiSegment GetMultiSegmentWithMinimum(MultiSegment segment, double step, Vector dir)
        {
            while (true)
            {
                var oldRes = _func(segment.To);
                var newRes = _func(segment.To + dir * step);

                if (newRes >= oldRes)
                    return new MultiSegment { From = segment.From, To = segment.To + dir * step };

                segment.From = segment.To;
                segment.To += segment.To + dir * step;
                step *= 2;
            }
        }
    }
}
