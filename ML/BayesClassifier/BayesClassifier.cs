using System;
using System.Linq;
using System.Collections.Generic;

namespace Bayes
{
    public class BayesClassifier
    {
        private readonly DataSet[] _dataSets;
        private readonly double _count;

        public static event Action<int> OnStartProcessing;
        public static event Action OnEndOfIteration;

        private readonly double _error = Math.Pow(10, -10);

        public BayesClassifier()
        {
            var dataReader = new DataReader();
            var dataParts = dataReader.Process();
            _count = dataReader.Count;

            _dataSets = dataParts
                .AsParallel()
                .Select((dp, i) => PrepareDataSet(dataParts.Without(i), dp))
                .ToArray();
        }
        private static DataSet PrepareDataSet(IList<DataPart> train, DataPart test)
        {
            var dataPart = new DataPart(train);
            return dataPart.ToDataSet(test);
        }

        public double FindWindowWidth()
        {
            var segment = new Segment {From = 1e-5, To = 1e-2};

            while (segment.Length > _error)
                segment = DichotomyMethod(segment);

            return segment.Mid;
        }
        private Segment DichotomyMethod(Segment segment)
        {
            var delta = _error / 2 * 0.99;

            var x1 = segment.Mid - delta;
            var x2 = segment.Mid + delta;

            return GetNextSegment(segment, x1, x2);
        }
        private Segment GetNextSegment(Segment segment, double x1, double x2)
        {
            var res1 = CrossValidation(1, 1, x1);
            var res2 = CrossValidation(1, 1, x2);

            if (res1.CompareTo(res2) == 0)
                return new Segment { From = x1, To = x2 };

            return res1.CompareTo(res2) > 0
                ? new Segment { From = segment.From, To = x2 }
                : new Segment { From = x1, To = segment.To };
        }

        public (double legitPenalty, double accuracy)[] GetResults()
        {
            var arr = GetArray();

            OnStartProcessing?.Invoke(arr.Length);

            return arr
                .Select(lp => (lp, CrossValidation(lp, 1, 0.002) / _count * 100))
                .ToArray();
        }
        private static double[] GetArray()
        {
            return Enumerable.Range(60, 100).Select(i => i / 100.0).ToArray();
        }

        public double FindLegitPenalty()
        {
            var result = _dataSets
                .Select(ds => ds.PredictLegit(1, 1, 0.002))
                .ToArray();

            return CrossValidation(1, 1, 0.002) / _count * 100;
        }

        private void SaveResult(ICollection<Result> results, double penaltyLegit, double penaltySpam, double windowWidth)
        {
            results.Add(new Result
            {
                LegitPenalty = penaltyLegit,
                SpamPenalty = penaltySpam,
                WindowWidth = windowWidth,
                Accuracy = CrossValidation(penaltyLegit, penaltySpam, windowWidth)
            });

            OnEndOfIteration?.Invoke();
        }
        private int CrossValidation(double penaltyLegit, double penaltySpam, double windowWidth)
        {
            OnEndOfIteration?.Invoke();

            return _dataSets
                .AsParallel()
                .Select(ds => ds.Predict(penaltyLegit, penaltySpam, windowWidth))
                .Sum();
        }
    }
}
