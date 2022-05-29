using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public static class Dispersion
    {
        public static void Process()
        {
            Console.ReadLine();
            var count = ReadInt();

            var x = new List<(int category, int value)>(count);

            for (int i = 0; i < count; i++)
                x.Add(ReadTuple());

            var res = x
                .GroupBy(v => v.category)
                .Select(c => c
                    .Select(v => v.value)
                    .AverageSquare() * c.Count())
                .Sum() / count;

            Console.WriteLine(res);
        }

        private static int ReadInt()
            => Console.ReadLine().ToInt();
        private static int[] ReadSeq()
            => Console.ReadLine().Split().Select(int.Parse).ToArray();

        private static (int, int) ReadTuple()
        {
            var seq = ReadSeq();
            return (seq[0], seq[1]);
        }
    }
}
