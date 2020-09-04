namespace Lab1_1.DTO
{
    public class Segment
    {
        public double From { get; set; }
        public double To { get; set; }

        public double Length => To - From;
        public double Mid => (From + To) / 2;
    }
}
