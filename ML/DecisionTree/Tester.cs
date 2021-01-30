using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DecisionTree
{
    public class Tester
    {
        private const string ResultFolder = @"res\";
        private const int TestSize = 15;

        private readonly IList<DataSet> _dataSets;

        public static event Action<int> OnStartProcessing;
        public static event Action OnEndOfIteration;

        public Tester()
        {
            _dataSets = DataReader.Read();
        }

        public void Run()
        {
            OnStartProcessing?.Invoke(_dataSets.Count);

            _dataSets
                .AsParallel()
                .ForAll(Process);
        }
        private static void Process(DataSet dataSet)
        {
            var results = new List<(int treeHeight, double accuracy)>(TestSize);

            for (int i = 0; i < 10; i++)
            {
                results.AddRange(TestTrees(dataSet, i * TestSize));

                if (!results.Max(res => res.accuracy).Equals(results.Last().accuracy))
                    break;
            }

            WriteResults(results, dataSet.Index);
        }
        private static IEnumerable<(int treeHeight, double accuracy)> TestTrees(DataSet dataSet, int heightOffset)
        {
            var results = new List<(int treeHeight, double accuracy)>(TestSize);

            for (int i = 1 + heightOffset; i < TestSize + heightOffset + 1; i++)
            {
                var tree = new DecisionTree(dataSet.Train, dataSet.ClassesCount, true, i);
                results.Add((i, tree.Test(dataSet.Test)));
            }

            return results;
        }

        private static void WriteResults(ICollection<(int treeHeight, double accuracy)> results, int dataSetNum)
        {
            using var sw = new StreamWriter($"{ResultFolder}{dataSetNum}.txt");

            sw.WriteLine(results.Count);
            foreach (var (treeHeight, accuracy) in results)
                sw.WriteLine($"{treeHeight} {accuracy}");

            OnEndOfIteration?.Invoke();
        }
    }
}
