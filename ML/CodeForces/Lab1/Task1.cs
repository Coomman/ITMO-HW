using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    public class Task1
    {
        private static void Main_()
        {
            var groupCount = int.Parse(Console.ReadLine().Split().Last());

            var dataSet = Console.ReadLine().Split()
                .Select((o, i) => (classNum: int.Parse(o), index: i + 1))
                .OrderBy(o => o.classNum)
                .Select(o => o.index)
                .ToList();

            var ans = Solve(groupCount, dataSet);

            foreach (var group in ans)
            {
                Console.Write($"{group.Count} ");
                Console.WriteLine(string.Join(" ", group));
            }
        }

        private static List<int>[] Solve(int groupCount, IReadOnlyList<int> dataSet)
        {
            var ans = new List<int>[groupCount];
            var size = (int) Math.Ceiling((double) dataSet.Count / groupCount);

            for (int i = 0; i < groupCount; i++)
                ans[i] = new List<int>(size);

            for (int i = 0; i < dataSet.Count; i++)
                ans[i % groupCount].Add(dataSet[i]);

            return ans;
        }
    }
}
