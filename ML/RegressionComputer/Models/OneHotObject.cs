using System.Collections.Generic;

namespace RegressionComputer.Models
{
    public class OneHotObject : IObject
    {
        public Vector Features { get; }
        public Vector Labels { get; }
        public int Class => Labels.ArgMax;
        public Dictionary<DistanceFunc, double[]> Distances { get; set; }

        public OneHotObject(Vector features, Vector labels)
        {
            Features = features;
            Labels = labels;
        }
    }
}
