using System;
using System.Linq;
using System.Collections.Generic;

namespace DecisionTree
{
    public class DecisionTree
    {
        private readonly int _classesCount;
        private readonly int _maxHeight;

        private readonly INode _root;
        private readonly Func<IList<Object>, int, int, Node> _splitFunc;

        public DecisionTree(IList<Object> objects, int classesCount, bool needPruning, int maxHeight = 0)
        {
            _classesCount = classesCount;
            _maxHeight = needPruning ? maxHeight : objects.Count;

            if (objects.Count < 200)
                _splitFunc = EntropySplit;
            else
                _splitFunc = GiniSplit;

            _root = ID3(objects, 0, objects.Count, 0);
        }

        #region Core

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
                        double curMetric = leftSum / (double)leftSize + rightSum / (double)rightSize;

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

        #endregion

        public int Classify(Object obj)
        {
            var curNode = _root;

            while (curNode is Node splitNode)
            {
                curNode = obj.Features[splitNode.FeatureIndex] < splitNode.SeparatingValue 
                    ? splitNode.Lch 
                    : splitNode.Rch;
            }

            return ((Leaf) curNode).ClassLabel;
        }

        public double Test(IList<Object> test)
        {
            var sum = test.Count(obj => Classify(obj) == obj.Label);

            return sum / (double) test.Count;
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
}
