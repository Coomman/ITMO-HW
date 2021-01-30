using System.Collections.Generic;

namespace Bayes
{
    public class Message
    {
        private readonly Dictionary<string, int> _header = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _body = new Dictionary<string, int>();

        private readonly int _headerSize;
        private readonly int _bodySize;

        public Message(string header, string body)
        {
            _headerSize = _header.AddAsWords(header);
            _bodySize = _body.AddAsWords(body);
        }

        public void AddToGlobal(Dictionary<string, int> globalHeader, Dictionary<string, int> globalBody)
        {
            foreach (var (key, value) in _header)
                globalHeader.AddOrUpdate(key, value);

            foreach (var (key, value) in _body)
                globalBody.AddOrUpdate(key, value);
        }

        public double GetHeaderPrevalence(string word)
        {
            return GetPrevalence(word, _header, _headerSize);
        }
        public double GetBodyPrevalence(string word)
        {
            return GetPrevalence(word, _body, _bodySize);
        }
        private static double GetPrevalence(string word, IReadOnlyDictionary<string, int> dict, double size)
        {
            if (dict.TryGetValue(word, out var value))
                return value / size;

            return 0;
        }
    }
}
