namespace Lab1_1.DTO
{
    public class MultiSegment
    {
        public Vector From { get; set; }
        public Vector To { get; set; }

        public override string ToString()
        {
            return $"From: {From}, To: {To}";
        }

        public Vector Mid => From + (To - From) * 0.5;
        public double Length => (To - From).Length;
    }
}
