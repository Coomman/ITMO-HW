using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public static class Pearson
    {
        public static double Run()
        {
            var count = ReadInt();

            var x = new List<int>(count);
            var y = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                int[] el = ReadSeq();
                x.Add(el[0]);
                y.Add(el[1]);
            }

            var xAvg = x.Average();
            var yAvg = y.Average();
            var deltaX = x.To(v => v - xAvg);
            var deltaY = y.To(v => v - yAvg);
            double res = deltaX.Zip(deltaY, (a, b) => a * b).Sum()
                         / Math.Sqrt(deltaX.Sum(v => Math.Pow(v, 2)) * deltaY.Sum(v => Math.Pow(v, 2)));

            return double.IsNaN(res) ? 0 : res;
        }

        private static int ReadInt()
            => Console.ReadLine().ToInt();
        private static int[] ReadSeq()
            => Console.ReadLine().Split().Select(int.Parse).ToArray();
    }
}
