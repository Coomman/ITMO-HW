using System;

namespace Lab1_1.DTO
{
    public class InitialData
    {
        public Func<double, double> Func { get; set; }
        public double From { get; set; }
        public double To { get; set; }
    }
}
