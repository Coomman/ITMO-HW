using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Bayes
{
    public class DataReader
    {
        private readonly object _locker = new object();

        private const string DataFolder = "data/";
        private readonly string[] _folders = Directory.GetDirectories(DataFolder);

        public int Count { get; private set; }

        public DataPart[] Process()
        {
            return _folders
                .AsParallel()
                .Select(ProcessFolder)
                .ToArray();
        }

        private DataPart ProcessFolder(string folderPath)
        {
            var files = Directory.GetFiles(folderPath);

            lock (_locker)
                Count += files.Length;

            var legitMessages = new List<Message>();
            var spamMessages = new List<Message>();
            files.ForEach(filePath => ReadFile(filePath, legitMessages, spamMessages));

            return new DataPart(legitMessages, spamMessages);
        }
        private static void ReadFile(string filePath, List<Message> legitMessages, List<Message> spamMessages)
        {
            var list = Path.GetFileName(filePath).Contains("legit") ? legitMessages : spamMessages;

            using var sr = new StreamReader(filePath);

            var header = new string(sr.ReadLine().Skip(9).ToArray());
            sr.ReadLine();
            var body = sr.ReadLine();

            list.Add(new Message(header, body));
        }
    }
}
