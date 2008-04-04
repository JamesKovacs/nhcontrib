using System;

namespace NHibernate.Burrow.Exceptions {
    public class IncorrectConversationSpanStatusException : Exception {
        public IncorrectConversationSpanStatusException() : base() {}
        public IncorrectConversationSpanStatusException(string msg) : base(msg) {}
    }
}