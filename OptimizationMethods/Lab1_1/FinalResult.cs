using System.Collections.Generic;

namespace OptimizationMethods
{
    public class FinalResult
    {
        public List<IterationResult> Results { get; set; }
        public int IterationCount { get; set; }
        public double Res { get; set; }
        public Methods Method { get; set; }
    }
}
