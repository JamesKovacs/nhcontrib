using System;

namespace NHibernate.Burrow.Exceptions {

    [Serializable]
    public class ConversationUninitializedException : BurrowException {
        public ConversationUninitializedException()
            : base("Conversation is not yet initialized for this thread yet, Either StartNew or Retrieve first") {}

        public ConversationUninitializedException(string msg) : base(msg) {}
    }
}