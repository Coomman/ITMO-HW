using System;
using System.Linq;

namespace Lab4
{
    public static class SpearMan
    {
        public static void Process()
        {
            long count = ReadInt();

            var x = new int[count];
            var y = new int[count];
            for (int i = 0; i < count; i++)
            {
                var seq = ReadSeq();
                x[i] = seq[0];
                y[i] = seq[1];
            }

            var res = 1.0 - 6.0 * x.CalculateRank().Zip(y.CalculateRank(), (a, b) => Math.Pow(a - b, 2))
                                     .Sum() / count / (count * count - 1);

            Console.WriteLine(res);
            Console.ReadLine();
        }

        private static int ReadInt()
            => Console.ReadLine().ToInt();
        private static int[] ReadSeq()
            => Console.ReadLine().Split().Select(int.Parse).ToArray();
    }
}
