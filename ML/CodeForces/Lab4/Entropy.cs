using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public class ObjectClass : IEnumerable<int>
    {
        private readonly Dictionary<int, int> _dict = new Dictionary<int, int>();

        public int Size { get; private set; }

        public ObjectClass(int yLabel)
        {
            AddOrUpdate(yLabel);
        }

        public void AddOrUpdate(int yLabel)
        {
            _dict.AddOrUpdate(yLabel);
            Size++;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return _dict.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class Entropy
    {
        public static void Process()
        {
            Console.ReadLine();
            int count = ReadInt();

            var dict = new Dictionary<int, ObjectClass>();

            for (int i = 0; i < count; i++)
            {
                var (x, y) = ReadTuple();

                dict.AddOrCreate(x - 1, y - 1);
            }

            double res = 0;
            foreach (var (x, xClass) in dict)
            {
                double sum = xClass.Sum(yCount => yCount / (double)xClass.Size * Math.Log(yCount / (double)xClass.Size));

                res += sum * xClass.Size / count;
            }

            Console.WriteLine(-res);
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
