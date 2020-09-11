﻿using System;
using System.Linq;
using System.Collections.Generic;

using Lab1_1.DTO;

namespace Lab1_1
{
    public enum Methods { Dichotomy, GoldenRatio, Fibonacci };

    public class MinimumFinder
    {
        private readonly ExcelHelper _logger = new ExcelHelper();
        private readonly List<long> _fibonacci = new List<long> {1, 1};

        private Func<double, double> _func;
        private Func<Segment, Segment> _method;

        private Segment _initialSegment;
        private IterationResult _lastIterationResult;
        private FinalResult _result;

        private double _error = Math.Pow(10, -5);
        private int _iterationCount;

        public MinimumFinder(Methods method)
        {
            _method = method switch
            {
                Methods.Dichotomy => DichotomyMethod,
                Methods.GoldenRatio => GoldenRatioMethod,
                Methods.Fibonacci => FibonacciMethod
            };
        }
        public MinimumFinder(ExcelHelper logger)
        {
            _logger = logger;
        }

        public FinalResult GetMinimum(InitialData data)
        {
            _func = data.Func;
            _iterationCount = 0;
            _result = new FinalResult {Results = new List<IterationResult>()};

            if (_method == FibonacciMethod)
                GenerateFibonacci((long) ((data.To - data.From) / _error));

            var segment = new Segment {From = data.From, To = data.To};
            _initialSegment = segment;
            _lastIterationResult = new IterationResult {Segment = _initialSegment};
            while (segment.Length >= _error)
            {
                _iterationCount++;
                segment = _method(segment);

                _result.Results.Add(_lastIterationResult);
            }

            _result.Results.Add(new IterationResult
            {
                Segment = segment, From = segment.From, To = segment.To,
                LengthRatio = segment.Length / _initialSegment.Length,
                Res1 = _func(segment.From), Res2 = _func(segment.To)
            });
            _result.IterationCount = _iterationCount;
            _result.Res = Math.Min(_func(segment.From), _func(segment.To));

            return _result;
        }
        public void RunAll(InitialData data, double error)
        {
            var results = new FinalResult[3];
            _error = error;

            _method = DichotomyMethod;
            results[0] = GetMinimum(data);
            results[0].Method = Methods.Dichotomy;

            _method = GoldenRatioMethod;
            results[1] = GetMinimum(data);
            results[1].Method = Methods.GoldenRatio;

            _method = FibonacciMethod;
            results[2] = GetMinimum(data);
            results[2].Method = Methods.Fibonacci;

            _logger.ProcessSheet(results, error);
        }

        private void GenerateFibonacci(long rightBorder)
        {
            while (_fibonacci.Last() <= rightBorder)
                _fibonacci.Add(_fibonacci[^1] + _fibonacci[^2]);
        }

        private Segment DichotomyMethod(Segment segment)
        {
            var delta = _error / 2 * 0.99;

            var x1 = segment.Mid - delta;
            var x2 = segment.Mid + delta;

            return GetNextSegment(segment, x1, x2);
        }
        private Segment GoldenRatioMethod(Segment segment)
        {
            var x1 = segment.From + 0.381966011 * (segment.To - segment.From);
            var x2 = segment.From + 0.618003399 * (segment.To - segment.From);

            return GetNextSegment(segment, x1, x2);
        }
        private Segment FibonacciMethod(Segment segment)
        {
            var factor = (_initialSegment.To - _initialSegment.From) / _fibonacci.Last();

            var x1 = segment.From + _fibonacci[^(_iterationCount + 2)] * factor;
            var x2 = segment.From + _fibonacci[^(_iterationCount + 1)] * factor;

            return GetNextSegment(segment, x1, x2);
        }

        private Segment GetNextSegment(Segment segment, double x1, double x2)
        {
            var res1 = _func(x1);
            var res2 = _func(x2);

            _lastIterationResult = new IterationResult
            {
                Segment = segment,
                LengthRatio = segment.Length / _initialSegment.Length,
                From = segment.From, To = segment.To, Res1 = res1, Res2 = res2
            };

            if (res1.CompareTo(res2) == 0)
                return new Segment { From = x1, To = x2 };

            return res1.CompareTo(res2) < 0
                ? new Segment { From = segment.From, To = x2 }
                : new Segment { From = x1, To = segment.To };
        }

        public Segment GetSegmentWithMinimum(Func<double, double> func)
        {
            const int startPoint = 0;
            _func = func;

            var segment = GetInitialSegment(startPoint);

            double step = _error;
            if (segment.To < segment.From)
                step = -step;

            return GetSegmentWithMinimum(segment, step);
        }

        private Segment GetInitialSegment(double startPoint)
        {
            var segment = new Segment { From = startPoint };

            if (_func(startPoint + _error) < _func(startPoint))
            {
                segment.To = startPoint + _error;
            }
            else if (_func(startPoint - _error) < _func(startPoint))
            {
                segment.To = startPoint - _error;
            }
            else
            {
                segment.From = startPoint - _error;
                segment.To = startPoint + _error;
            }

            return segment;
        }
        private Segment GetSegmentWithMinimum(Segment segment, double step)
        {
            while (true)
            {
                var oldRes = _func(segment.To);
                var newRes = _func(segment.To + step);

                if (newRes >= oldRes)
                    return new Segment
                    {
                        From = Math.Min(segment.From, segment.To + step),
                        To = Math.Max(segment.From, segment.To + step)
                    };

                segment.From = segment.To;
                segment.To += step;
                step *= 2;
            }
        }
    }
}