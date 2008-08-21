using System;

namespace NHibernate.Burrow.Exceptions
{
    [Serializable]
    public class ConversationUnavailableException : BurrowException
    {
        public ConversationUnavailableException() : this("Either workspace is not initialized yet or it is closed") {}
        public ConversationUnavailableException(string msg) : base(msg) {}
    }
}