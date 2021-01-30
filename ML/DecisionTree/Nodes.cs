namespace DecisionTree
{
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
}
