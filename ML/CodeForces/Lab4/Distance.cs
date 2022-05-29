using System;
using System.Linq;

namespace Lab4
{
    public class Distance
    {
        public static void Process()
        {
            var classCount = ReadInt();
            var count = ReadInt();

            var innerDist = Enumerable.Repeat((x: 0L, y: 0L), classCount).ToList();
            var outerDist = (x: 0L, y: 0L);

            long innerSum = 0;
            long outerSum = 0;

            var objects = new (int x, int y)[count];
            for (var i = 0; i < count; i++)
                objects[i] = ReadTuple();

            objects = objects.OrderBy(v => v.x).ToArray();

            foreach (var (x, y) in objects)
            {
                outerDist = (outerDist.x + 1, outerDist.y + x);
                outerSum += outerDist.x * x - outerDist.y;

                innerDist[y] = (innerDist[y].x + 1, innerDist[y].y + x);
                innerSum += innerDist[y].x * x - innerDist[y].y;
            }

            Console.WriteLine(innerSum * 2);
            Console.WriteLine((outerSum - innerSum) * 2);
        }

        private static int ReadInt()
            => Console.ReadLine().ToInt();
        private static int[] ReadSeq()
            => Console.ReadLine().Split().Select(int.Parse).ToArray();
        private static (int, int) ReadTuple()
        {
            var seq = ReadSeq();
            return (seq[0], seq[1] - 1);
        }
    }
}
