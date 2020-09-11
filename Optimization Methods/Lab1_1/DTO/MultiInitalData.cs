using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1_1.DTO
{
    public class MultiInitalData
    {
        public Func<Vector, double> Func { get; set; }
        public Vector From { get; set; }
        public Vector To { get; set; }
    }
}
