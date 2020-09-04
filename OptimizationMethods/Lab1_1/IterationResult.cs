namespace OptimizationMethods
{
    public class IterationResult
    {
        public Segment Segment { get; set; }
        public double LengthRatio { get; set; }
        
        public double From { get; set; }
        public double To { get; set; }
        public double Res1 { get; set; }
        public double Res2 { get; set; }
    }
}
