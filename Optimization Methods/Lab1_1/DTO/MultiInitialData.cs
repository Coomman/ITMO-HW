using System;

namespace Lab1_1.DTO
{
    public class MultiInitialData
    {
        public Func<Vector, double> Func { get; set; }
        public Vector From { get; set; }
        public Vector To { get; set; }
    }
}
