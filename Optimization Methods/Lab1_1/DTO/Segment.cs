using System;

namespace Lab1_1.DTO
{
    public class Segment
    {
        public double From { get; set; }
        public double To { get; set; }

        public double Length => Math.Abs(To - From);
        public double Mid => (From + To) / 2;

        public override string ToString()
            => $"From = {From}, To = {To}";
    }
}
