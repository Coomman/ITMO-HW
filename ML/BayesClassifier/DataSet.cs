using System;
using System.Collections.Generic;
using System.Linq;

namespace Bayes
{
    public class DataSet
    {
        private readonly IList<string> _headerDict;
        private readonly IList<string> _bodyDict;

        private readonly MessageClass _trainLegit;
        private readonly MessageClass _trainSpam;

        private readonly IList<Message> _testLegit;
        private readonly IList<Message> _testSpam;

        private static readonly Func<double, double> KernelFunc = u => Math.Exp(-0.5 * u * u) / Math.Sqrt(2 * Math.PI);

        public DataSet(IList<string> headerDict, IList<string> bodyDict, MessageClass trainLegit,
            MessageClass trainSpam, IList<Message> testLegit, IList<Message> testSpam)
        {
            _headerDict = headerDict;
            _bodyDict = bodyDict;
            _trainLegit = trainLegit;
            _trainSpam = trainSpam;
            _testLegit = testLegit;
            _testSpam = testSpam;
        }

        public double PredictLegit(double penaltyLegit, double penaltySpam, double windowWidth)
        {
            var legitCorrectForecast = _testLegit
                .AsParallel()
                .Select(msg => Predict(msg, _trainLegit, penaltyLegit, windowWidth))
                .ToArray();
            var legitIncorrectForecast = _testLegit
                .AsParallel()
                .Select(msg => Predict(msg, _trainSpam, penaltySpam, windowWidth))
                .ToArray();

            return legitCorrectForecast.GetTP(legitIncorrectForecast) / (double) _testLegit.Count * 100;
        }

        public int Predict(double penaltyLegit, double penaltySpam, double windowWidth)
        {
            var legitCorrectForecast = _testLegit
                .AsParallel()
                .Select(msg => Predict(msg, _trainLegit, penaltyLegit, windowWidth))
                .ToArray();
            var legitIncorrectForecast = _testLegit
                .AsParallel()
                .Select(msg => Predict(msg, _trainSpam, penaltySpam, windowWidth))
                .ToArray();

            var spamCorrectForecast = _testSpam
                .AsParallel()
                .Select(msg => Predict(msg, _trainSpam, penaltySpam, windowWidth))
                .ToArray();
            var spamIncorrectForecast = _testSpam
                .AsParallel()
                .Select(msg => Predict(msg, _trainLegit, penaltyLegit, windowWidth))
                .ToArray();

            return legitCorrectForecast.GetTP(legitIncorrectForecast) + spamCorrectForecast.GetTP(spamIncorrectForecast);
        }
        private double Predict(Message testMessage, MessageClass msgClass, double penalty, double windowWidth)
        {
            return Math.Log(penalty * msgClass.Prevalence)
                   + _headerDict.Sum(word =>
                       ParsenRosenblatt(word, testMessage.GetHeaderPrevalence(word), msgClass, windowWidth, msg => msg.GetHeaderPrevalence))
                   + _bodyDict.Sum(word =>
                       ParsenRosenblatt(word, testMessage.GetBodyPrevalence(word), msgClass, windowWidth, msg => msg.GetBodyPrevalence));
        }
        private static double ParsenRosenblatt(string word, double prevalence, MessageClass msgClass, double windowWidth, Func<Message, Func<string, double>> prev)
        {
            var likeliness = msgClass.Sum(msg => KernelFunc(Math.Abs(prevalence - prev(msg).Invoke(word)) / windowWidth));

            return Math.Log(likeliness / msgClass.Size);
        }
    }
}
