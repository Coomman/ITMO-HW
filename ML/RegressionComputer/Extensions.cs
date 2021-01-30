using System;
using System.Linq;
using System.Collections.Generic;

using RegressionComputer.Models;

namespace RegressionComputer
{
    public static class Extensions
    {
        public static int ArgMax<T>(this IList<T> list) where T: IComparable
        {
            var maxEl = list.First();
            var index = 0;

            for (int i = 0; i < list.Count; i++)
                if (list[i].CompareTo(maxEl) > 0)
                {
                    maxEl = list[i];
                    index = i;
                }

            return index;
        }

        public static IEnumerable<T> Without<T>(this IList<T> list, T element)
        {
            return list.Where(el => !el.Equals(element));
        }

        public static Vector Sum(this IEnumerable<Vector> list)
        {
            var vectors = list.ToList();

            var sum = Vector.GetNullVector(vectors.First().Size);

            return vectors.Aggregate(sum, (current, vec) => current + vec);
        }
    }
}
