using System;
using System.Collections.Generic;

namespace DecisionTree
{
    public static class Extensions
    {
        public static int ToInt(this string str)
        {
            return int.Parse(str);
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

        public static void Swap<T>(this IList<T> arr, int first, int second)
        {
            var temp = arr[first];
            arr[first] = arr[second];
            arr[second] = temp;
        }

        public static void AddOrUpdate<TKey>(this Dictionary<TKey, int> dict, TKey key, int diff = 1)
        {
            if (dict.ContainsKey(key))
                dict[key] += diff;
            else
                dict[key] = diff;
        }
        public static int GetMajorityClass(this IList<Object> objects, int left, int right)
        {
            var dict = new Dictionary<int, int> { [0] = 0 };
            int majorityClass = 0;

            for (int i = left; i < right; i++)
            {
                dict.AddOrUpdate(objects[i].Label);

                if (dict[objects[i].Label] > dict[majorityClass])
                    majorityClass = objects[i].Label;
            }

            return majorityClass;
        }
        public static bool BelongToOneClass(this IList<Object> objects, int left, int right, out int classLabel)
        {
            classLabel = objects[left].Label;

            for (int i = left; i < right; i++)
                if (objects[i].Label != classLabel)
                    return false;

            return true;
        }

        public static void InsertionSort(this IList<Object> arr, int left, int right, Func<Object, Object, int> compare)
        {
            for (int i = left + 1; i <= right; i++)
            {
                var cur = arr[i];
                int j = i - 1;

                while (j >= left && compare(arr[j], cur) > 0)
                    arr[j + 1] = arr[j--];

                arr[j + 1] = cur;
            }
        }
        public static void QuickSort(this IList<Object> arr, int left, int right, Func<Object, Object, int> compare)
        {
            if (right <= left)
                return;

            if (right - left + 1 <= 80)
            {
                InsertionSort(arr, left, right, compare);
                return;
            }

            var (leftBound, rightBound) = Partition(arr, left, right, compare);

            QuickSort(arr, left, leftBound - 1, compare);
            QuickSort(arr, rightBound + 1, right, compare);
        }
        private static (int leftBound, int rightBound) Partition(this IList<Object> arr, int left, int right, Func<Object, Object, int> compare)
        {
            arr.SetPivot(left, right, compare);

            int l = left;
            int r = right;
            var pivot = arr[left];
            int cur = left + 1;

            while (cur <= r)
            {
                int cmp = compare(arr[cur], pivot);

                if (cmp < 0)
                    arr.Swap(l++, cur++);
                else if (cmp > 0)
                    arr.Swap(cur, r--);
                else
                    cur++;
            }

            return (l, r);
        }
        private static void SetPivot(this IList<Object> arr, int l, int r, Func<Object, Object, int> compare)
        {
            int mid = l + (r - l) / 2;

            arr.Swap(mid, l + 1);
            arr.Swap(r, l + 2);

            arr.InsertionSort(l, l + 2, compare);

            arr.Swap(l, l + 1);
        }

        public static int Partition(this IList<Object> arr, int left, int right, Func<Object, bool> isLeft)
        {
            int leftIt = left, rightIt = right - 1;

            while (leftIt < rightIt)
            {
                if (isLeft(arr[leftIt]))
                    leftIt++;
                else
                {
                    arr.Swap(leftIt, rightIt);
                    rightIt--;
                }
            }

            return isLeft(arr[leftIt]) ? leftIt + 1 : leftIt;
        }
    }
}
