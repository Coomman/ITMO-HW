using System;

namespace OptimizationMethods
{
    public class InitialData
    {
        public Func<double, double> Func { get; set; }
        public double From { get; set; }
        public double To { get; set; }
    }
}
