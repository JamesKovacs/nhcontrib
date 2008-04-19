using System;

namespace NHibernate.Burrow.Exceptions
{
    [Serializable]
    public class ConversationUnavailableException : BurrowException
    {
        public ConversationUnavailableException() : base() {}
        public ConversationUnavailableException(string msg) : base(msg) {}
    }
}