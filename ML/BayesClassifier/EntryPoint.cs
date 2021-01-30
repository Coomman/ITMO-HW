using System;
using System.IO;

namespace Bayes
{
    public class EntryPoint
    {
        private static void Main()
        {
            AddProgressBar();

            var bayes = new BayesClassifier();
            var results = bayes.GetResults();

            using var sw = new StreamWriter("res.txt");
            foreach (var (legitPenalty, accuracy) in results)
                sw.WriteLine($"{legitPenalty} {accuracy}");
        }   

        private static void AddProgressBar()
        {
            var progressBar = new ProgressBar();

            BayesClassifier.OnStartProcessing += progressBar.Start;
            BayesClassifier.OnEndOfIteration += progressBar.Lap;
        }
    }
}
