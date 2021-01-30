using System.Collections;
using System.Collections.Generic;

namespace Bayes
{
    public class MessageClass : IEnumerable<Message>
    {
        private readonly IList<Message> _messages;
        
        public double Prevalence { get; }
        public double Size { get; }

        public MessageClass(IList<Message> messages, int size)
        {
            _messages = messages;
            Size = messages.Count;
            Prevalence = messages.Count / (double) size;
        }

        public IEnumerator<Message> GetEnumerator()
        {
            return _messages.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
