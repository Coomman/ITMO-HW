using System;
using System.Linq;
using System.Collections.Generic;

namespace Lab3
{
    static class Extensions
    {
        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }

        public static void Swap<T>(this IList<T> arr, int first, int second)
        {
            var temp = arr[first];
            arr[first] = arr[second];
            arr[second] = temp;
        }

        public static void AddOrUpdate<TKey>(this Dictionary<TKey, double> dict, TKey key)
        {
            if (dict.ContainsKey(key))
                dict[key] += 1;
            else
                dict[key] = 1;
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

    public class Object
    {
        public IReadOnlyList<int> Features { get; }
        public int Label { get; }

        public Object(IReadOnlyList<int> features, int label)
        {
            Features = features;
            Label = label;
        }

        public override string ToString()
        {
            return $"{{{string.Join(", ", Features)}}} => {Label}";
        }
    }

    public interface INode { }
    public class Node : INode
    {
        public int FeatureIndex { get; }
        public double SeparatingValue { get; }

        public INode Lch;
        public INode Rch;

        public Node(int featureIndex, double separatingValue)
        {
            FeatureIndex = featureIndex;
            SeparatingValue = separatingValue;
        }
    }
    public class Leaf : INode
    {
        public int ClassLabel { get; }

        public Leaf(int classLabel)
        {
            ClassLabel = classLabel;
        }
    }

    public class DecisionTree
    {
        private readonly INode _root;
        private readonly int _classesCount;
        private readonly int _maxHeight;

        private readonly Func<IList<Object>, int, int, Node> _splitFunc;

        public DecisionTree(IList<Object> objects, int classesCount, int maxHeight)
        {
            _classesCount = classesCount;
            _maxHeight = maxHeight;

            if (objects.Count < 200)
                _splitFunc = EntropySplit;
            else
                _splitFunc = GiniSplit;

            _root = ID3(objects, 0, objects.Count, 0);
        }

        private INode ID3(IList<Object> objects, int left, int right, int curHeight)
        {
            if (curHeight == _maxHeight)
                return new Leaf(objects.GetMajorityClass(left, right));

            if (objects.BelongToOneClass(left, right, out var label))
                return new Leaf(label);

            var splitNode = _splitFunc(objects, left, right);

            if (splitNode is null)
                return new Leaf(objects.GetMajorityClass(left, right));

            var mid = objects.Partition(left, right, obj => obj.Features[splitNode.FeatureIndex] < splitNode.SeparatingValue);

            splitNode.Lch = ID3(objects, left, mid, curHeight + 1);
            splitNode.Rch = ID3(objects, mid, right, curHeight + 1);

            return splitNode;
        }

        private Node EntropySplit(IList<Object> objects, int left, int right)
        {
            Node res = null;
            double maxMetric = 0;

            var defCount = new int[_classesCount];
            for (int it = left; it < right; it++)
                defCount[objects[it].Label - 1]++;

            var defEntropy = Entropy(defCount, right - left) * (right - left);

            for (int i = 0; i < objects[left].Features.Count; i++)
            {
                int featureIndex = i;
                objects.QuickSort(left, right - 1, (l, r) => l.Features[featureIndex].CompareTo(r.Features[featureIndex]));

                if (objects[left].Features[featureIndex] == objects[right - 1].Features[featureIndex])
                    continue;

                var leftCount = new int[_classesCount];
                var rightCount = new int[_classesCount];
                int leftSize = 0;
                int rightSize = right - left;

                for (int it = left; it < right; it++)
                    rightCount[objects[it].Label - 1]++;

                int prevFeature = 0;
                for (var mid = left; mid < right; mid++)
                {
                    if (mid != left && objects[mid].Features[featureIndex] != prevFeature)
                    {
                        double curMetric = defEntropy - Entropy(leftCount, leftSize) * leftSize - Entropy(rightCount, rightSize) * rightSize;

                        if (curMetric > maxMetric)
                        {
                            res = new Node(featureIndex, (prevFeature + objects[mid].Features[featureIndex]) / 2.0);
                            maxMetric = curMetric;
                        }
                    }

                    leftCount[objects[mid].Label - 1]++;
                    rightCount[objects[mid].Label - 1]--;

                    leftSize++;
                    rightSize--;

                    prevFeature = objects[mid].Features[featureIndex];
                }
            }

            return res;

            static double Entropy(IEnumerable<int> count, double size)
            {
                return count
                    .Where(classCount => classCount != 0)
                    .Aggregate(0.0, (current, classCount) => current - classCount / size * Math.Log2(classCount / size));
            }
        }
        private Node GiniSplit(IList<Object> objects, int left, int right)
        {
            Node res = null;
            double maxMetric = 0;

            for (int i = 0; i < objects[left].Features.Count; i++)
            {
                int featureIndex = i;
                objects.QuickSort(left, right - 1, (l, r) => l.Features[featureIndex].CompareTo(r.Features[featureIndex]));

                if (objects[left].Features[featureIndex] == objects[right - 1].Features[featureIndex])
                    continue;

                var leftCount = new int[_classesCount];
                var rightCount = new int[_classesCount];
                int leftSize = 0;
                int rightSize = right - left;
                long leftSum = 0;
                long rightSum = 0;

                for (int it = left; it < right; it++)
                    Update(ref rightCount[objects[it].Label - 1], ref rightSum, 1);

                int prevFeature = 0;
                for (var mid = left; mid < right; mid++)
                {
                    if (mid != left && objects[mid].Features[featureIndex] != prevFeature)
                    {
                        double curMetric = leftSum / (double) leftSize + rightSum / (double) rightSize;

                        if (curMetric > maxMetric)
                        {
                            res = new Node(featureIndex, (prevFeature + objects[mid].Features[featureIndex]) / 2.0);
                            maxMetric = curMetric;
                        }
                    }

                    Update(ref rightCount[objects[mid].Label - 1], ref rightSum, -1);
                    Update(ref leftCount[objects[mid].Label - 1], ref leftSum, 1);

                    leftSize++;
                    rightSize--;

                    prevFeature = objects[mid].Features[featureIndex];
                }
            }

            return res;

            static void Update(ref int val, ref long sum, int delta)
            {
                sum -= val * val;
                val += delta;
                sum += val * val;
            }
        }

        public void Print()
        {
            var printNodes = new List<string>();

            var nodes = new Queue<INode>();
            nodes.Enqueue(_root);

            var nextNode = 2;
            while (nodes.Any())
            {
                var node = nodes.Dequeue();

                if (node is Node decisionNode)
                {
                    printNodes.Add($"Q {decisionNode.FeatureIndex + 1} {decisionNode.SeparatingValue} {nextNode++} {nextNode++}");
                    nodes.Enqueue(decisionNode.Lch);
                    nodes.Enqueue(decisionNode.Rch);
                }
                else if (node is Leaf leaf)
                {
                    printNodes.Add($"C {leaf.ClassLabel}");
                }
            }

            Console.WriteLine(printNodes.Count);
            foreach (var node in printNodes)
                Console.WriteLine(node);
        }
    }

    internal class EntryPoint
    {
        private static void Main()
        {
            var info = Console.ReadLine().Split().Select(int.Parse).ToArray();
            var objCount = Console.ReadLine().ToInt();

            var objects = new Object[objCount];
            for (int i = 0; i < objCount; i++)
            {
                var objInfo = Console.ReadLine().Split().Select(int.Parse).ToArray();
                objects[i] = new Object(objInfo.Take(objInfo.Length - 1).ToArray(), objInfo.Last());
            }

            var tree = new DecisionTree(objects, info[1], info[2]);
            tree.Print();

            Console.ReadLine();
        }
    }
}
