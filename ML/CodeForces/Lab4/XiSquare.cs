using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public class XiSquare
    {
        public static void Process()
        {
            Console.ReadLine();
            int count = ReadInt();

            var xCounts = new Dictionary<int, double>();
            var yCounts = new Dictionary<int, double>();
            var dict = new Dictionary<(int x, int y), int>();

            for (int i = 0; i < count; i++)
            {
                var (x, y) = ReadTuple();

                xCounts.AddOrUpdate(x);
                yCounts.AddOrUpdate(y);
                dict.AddOrUpdate((x, y));
            }

            xCounts.ForEachValue(n => n / count);
            yCounts.ForEachValue(n => n / count);

            double res = count;
            foreach (var ((x, y), objCount) in dict)
            {
                double k = count * xCounts[x] * yCounts[y];

                res -= k;
                res += (objCount - k) * (objCount - k) / k;
            }

            Console.WriteLine(res);
        }

        private static int ReadInt()
            => Console.ReadLine().ToInt();
        private static int[] ReadSeq()
            => Console.ReadLine().Split().Select(int.Parse).ToArray();
        private static (int, int) ReadTuple()
        {
            var seq = ReadSeq();
            return (seq[0] - 1, seq[1] - 1);
        }
    }
}
