using System;

namespace NHibernate.Burrow.Exceptions {
    public class ConversationAlreadyCancelledException : BurrowException {
        public ConversationAlreadyCancelledException() : base() {}
        public ConversationAlreadyCancelledException(string msg) : base(msg) {}
    }
}