using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3
{
    public class MessageClass
    {
        private readonly Dictionary<string, double> _dict = new Dictionary<string, double>();
        private double _defaultProbability;

        public int Penalty { get; set; }
        public int Size { get; set; }

        public void Add(string word)
        {
            _dict.AddOrUpdate(word);
        }
        public void CountProbability(int alpha)
        {
            var keys = _dict.Keys.ToList();
            double divider = Size + 2 * alpha;

            foreach (var key in keys)
                _dict[key] = (_dict[key] + alpha) / divider;

            _defaultProbability = alpha / divider;
        }

        public double GetProbability(string word)
        {
            return _dict.TryGetValue(word, out var value)
                ? value
                : _defaultProbability;
        }
    }

    public class Bayes
    {
        private readonly int _classesCount;
        private readonly int[] _penalties;
        private readonly int _alpha;

        private readonly int _trainSize;
        private readonly MessageClass[] _train;

        private readonly HashSet<string> _dict = new HashSet<string>();

        public Bayes()
        {
            _classesCount = ReadInt();
            _penalties = ReadSeq();
            _alpha = ReadInt();

            _train = Enumerable.Repeat(0, _classesCount).Select(el => new MessageClass()).ToArray();

            _trainSize = ReadInt();
            ReadTrain();
        }
        private static int ReadInt()
            => Console.ReadLine().ToInt();
        private static int[] ReadSeq()
            => Console.ReadLine().Split().Select(int.Parse).ToArray();
        private void ReadTrain()
        {
            for (int i = 0; i < _trainSize; i++)
            {
                var msgInfo = Console.ReadLine().Split().ToArray();
                var label = msgInfo[0].ToInt() - 1;
                var words = new HashSet<string>(msgInfo.Skip(2));

                foreach (var word in words)
                {
                    _train[label].Add(word);
                    _dict.Add(word);
                }

                _train[label].Size++;
            }

            for (int i = 0; i < _classesCount; i++)
            {
                _train[i].Penalty = _penalties[i];
                _train[i].CountProbability(_alpha);
            }
        }

        public void Process()
        {
            var testSize = ReadInt();

            for (int i = 0; i < testSize; i++)
            {
                var words = new HashSet<string>(Console.ReadLine().Split().Skip(1));
                Console.WriteLine(string.Join(" ", Predict(words)));
            }
        }
        private IList<double> Predict(ICollection<string> testMsg)
        {
            var hasAnswer = new bool[_classesCount];
            var answers = new double[_classesCount];

            for (int i = 0; i < _classesCount; i++)
            {
                if (_train[i].Size == 0)
                    continue;

                answers[i] = Math.Log(_train[i].Size / (double)_trainSize * _train[i].Penalty);
                foreach (var word in _dict)
                    answers[i] += testMsg.Contains(word) ? Math.Log(_train[i].GetProbability(word)) : Math.Log(1 - _train[i].GetProbability(word));

                hasAnswer[i] = true;
            }

            var max = answers
                .Where((a, i) => hasAnswer[i] is true)
                .Max();

            for (int i = 0; i < _classesCount; i++)
                if (hasAnswer[i])
                    answers[i] = Math.Exp(answers[i] - max);

            var sum = answers.Sum();
            if (sum.CompareTo(0) == 0)
                return new double[_classesCount];

            for (int i = 0; i < _classesCount; i++)
                if (hasAnswer[i])
                    answers[i] /= sum;

            return answers;
        }
    }

    public class Prog
    {
        public static void _Main()
        {
            var bayes = new Bayes();
            
            bayes.Process();
        }
    }
}
