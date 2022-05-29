using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public static class Extensions
    {
        public static double AverageSquare(this IEnumerable<int> arr)
        {
            var list = arr.ToArray();
            var avg = list.Average();

            return list
                .Select(x => Math.Pow(x - avg, 2))
                .Average();
        }

        public static int ToInt(this string str)
            => int.Parse(str);

        public static List<T2> To<T1, T2>(this IList<T1> list, Func<T1, T2> func)
            => list.Select(func).ToList();

        public static void AddOrUpdate<T>(this Dictionary<T, double> dict, T key)
        {
            if (dict.ContainsKey(key))
                dict[key] += 1;
            else
                dict[key] = 1;
        }
        public static void AddOrUpdate<T>(this Dictionary<T, int> dict, T key)
        {
            if (dict.ContainsKey(key))
                dict[key] += 1;
            else
                dict[key] = 1;
        }

        public static void ForEachValue<T>(this Dictionary<T, double> dict, Func<double, double> func)
        {
            var keys = dict.Keys.ToArray();

            foreach (var key in keys)
                dict[key] = func(dict[key]);
        }

        public static IList<long> CalculateRank(this IList<int> arr)
        {
            var sorted = arr
                .Select((v, i) => (Value: v, Index: i))
                .OrderBy(t => t.Value)
                .ToArray();

            var ranks = new long[arr.Count];
            int rank = 0;

            ranks[sorted[0].Index] = rank;
            for (int i = 1; i < arr.Count; i++)
            {
                if (sorted[i].Value != sorted[i - 1].Value)
                    rank++;

                ranks[sorted[i].Index] = rank;
            }

            return ranks;
        }

        public static void AddOrCreate(this Dictionary<int, ObjectClass> dict, int key, int value)
        {
            if (dict.ContainsKey(key))
                dict[key].AddOrUpdate(value);
            else
                dict[key] = new ObjectClass(value);
        }
    }

    class EntryPoint
    {
        private static void Main()
        {
            Entropy.Process();

            Console.ReadLine();
        }
    }
}
