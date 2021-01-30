using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionTree
{
    public class Object
    {
        public IReadOnlyList<int> Features { get; }
        public int Label { get; }

        public Object(IReadOnlyList<int> features, int label)
        {
            Features = features;
            Label = label;
        }
    }
}
