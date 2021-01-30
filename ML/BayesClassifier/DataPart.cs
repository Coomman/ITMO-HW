using System.Linq;
using System.Collections.Generic;

namespace Bayes
{
    public class DataPart
    {
        private readonly IList<Message> _legitMessages;
        private readonly IList<Message> _spamMessages;

        private readonly Dictionary<string, int> _header = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _body = new Dictionary<string, int>();

        public DataPart(IList<Message> legitMessages, IList<Message> spamMessages)
        {
            _legitMessages = legitMessages;
            _spamMessages = spamMessages;

            _legitMessages.ForEach(msg => msg.AddToGlobal(_header, _body));
            _spamMessages.ForEach(msg => msg.AddToGlobal(_header, _body));
        }
        public DataPart(IList<DataPart> dataParts)
        {
            _legitMessages = dataParts.SelectMany(dp => dp._legitMessages).ToArray();
            _spamMessages = dataParts.SelectMany(dp => dp._spamMessages).ToArray();

            dataParts.ForEach(dp => _header.Unite(dp._header));
            dataParts.ForEach(dp => _body.Unite(dp._body));
        }

        public DataSet ToDataSet(DataPart test)
        {
            return new DataSet
            (
                headerDict: _header.GetPopularSegment(5, 10),
                bodyDict: _body.GetPopularSegment(21, 90),
                trainLegit: new MessageClass(_legitMessages, Size),
                trainSpam: new MessageClass(_spamMessages, Size),
                testLegit: test._legitMessages,
                testSpam: test._spamMessages
            );
        }

        private int Size => _legitMessages.Count + _spamMessages.Count;
    }
}
