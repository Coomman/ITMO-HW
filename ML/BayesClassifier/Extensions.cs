using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bayes
{
    static class Extensions
    {
        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }

        public static void ForEach<T>(this IEnumerable<T> arr, Action<T> action)
        {
            foreach (var el in arr)
                action(el);
        }

        public static int AddAsWords(this Dictionary<string, int> dict, string str, int n = 3)
        {
            var count = str.Length - 3;

            for (int i = 0; i < count; i++)
                dict.AddOrUpdate(str.Substring(i, n));

            return count;
        }
        public static void AddOrUpdate(this Dictionary<string, int> dict, string key, int diff = 1)
        {
            if (dict.ContainsKey(key))
                dict[key] += diff;
            else
                dict[key] = diff;
        }
        public static void Unite(this Dictionary<string, int> dict, Dictionary<string, int> other)
        {
            foreach (var (key, value) in other)
                dict.AddOrUpdate(key, value);
        }
        public static IList<string> GetPopularSegment(this Dictionary<string, int> dict, int skip, int take)
        {
            return dict
                .OrderByDescending(pair => pair.Value)
                .Skip(skip)
                .Take(take)
                .Select(pair => pair.Key)
                .ToArray();
        }

        public static void WriteFile(this Dictionary<string, int> dict, StreamWriter sw)
        {
            sw.WriteLine(dict.Count);

            foreach (var (key, value) in dict)
                sw.WriteLine($"{key}_{value}");
        }
        public static void ReadFile(this Dictionary<string, int> dict, StreamReader sr)
        {
            var count = sr.ReadLine().ToInt();

            for (int i = 0; i < count; i++)
            {
                var query = sr.ReadLine().Split('_');
                dict.Add(query[0], query[1].ToInt());
            }
        }

        public static T[] Without<T>(this IList<T> list, int index)
        {
            return list.Where((el, i) => i != index).ToArray();
        }
        public static int GetTP(this IList<double> correctForecast, IList<double> incorrectForecast)
        {
            return correctForecast
                .Where((cf, i) => cf > incorrectForecast[i])
                .Count();
        }

        public static void Write(this ConsoleColor color, string str)
        {
            ColorizeWriting(color, str, false);
        }
        public static void WriteLine(this ConsoleColor color, string str)
        {
            ColorizeWriting(color, str, true);
        }
        private static void ColorizeWriting(ConsoleColor color, string str, bool needWriteLine)
        {
            var previousColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.Write(str);

            if (needWriteLine)
                Console.WriteLine();

            Console.ForegroundColor = previousColor;
        }
    }
}
