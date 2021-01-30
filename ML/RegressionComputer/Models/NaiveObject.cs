using System.Collections.Generic;

namespace RegressionComputer.Models
{
    public class NaiveObject : IObject
    {
        public Vector Features { get; }
        public Vector Labels { get; }
        public int Class => (int) Labels[0];
        public Dictionary<DistanceFunc, double[]> Distances { get; set; }

        public NaiveObject(Vector features, Vector labels)
        {
            Features = features;
            Labels = labels;
        }
    }
}
