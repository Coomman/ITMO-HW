using System;
using System.Collections.Generic;
using Lab1_1.DTO;

namespace Lab1_1
{
    public class MultiMinimumFinder
    {
        private readonly List<long> _fibonacci = new List<long> { 1, 1 };

        private Func<Vector, double> _func;
        private Func<MultiSegment, MultiSegment> _method;

        private MultiSegment _initialSegment;

        private double _error = Math.Pow(10, -5);
        private int _iterationCount;

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
