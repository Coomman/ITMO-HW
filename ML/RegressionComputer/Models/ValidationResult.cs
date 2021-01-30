namespace RegressionComputer.Models
{
    internal class ValidationResult
    {
        public DistanceFunc DistanceFunc { get; set; }
        public KernelFunc KernelFunc { get; set; }
        public int NeighborCount { get; set; }
        public double F1Score { get; set; }

        public override string ToString()
            => $"{DistanceFunc} {KernelFunc} NeighborCount={NeighborCount} F1Score={F1Score}";
    }
}
