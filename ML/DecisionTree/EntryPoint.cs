namespace DecisionTree
{
    class EntryPoint
    {
        private static void Main()
        {
            AddProgressBar();

            var tester = new Tester();
            tester.Run();
        }

        private static void AddProgressBar()
        {
            var progressBar = new ProgressBar();

            Tester.OnStartProcessing += progressBar.Start;
            Tester.OnEndOfIteration += progressBar.Lap;
        }
    }
}
