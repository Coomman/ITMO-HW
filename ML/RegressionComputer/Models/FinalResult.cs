namespace RegressionComputer.Models
{
    public class FinalResult
    {
        public string DistanceFunc { get; set; }
        public string KernelFunc { get; set; }
        public double[] F1Score { get; set; }
        public string ElapsedTime { get; set; }
        public int ValidationCount { get; set; }
    }
}
