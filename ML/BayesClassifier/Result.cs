namespace Bayes
{
    public class Result
    {
        public double LegitPenalty { get; set; }
        public double SpamPenalty { get; set; }
        public double WindowWidth { get; set; }

        public double Accuracy { get; set; }

        public override string ToString()
        {
            return $"{Accuracy} {LegitPenalty} {SpamPenalty} {WindowWidth}";
        }
    }
}
