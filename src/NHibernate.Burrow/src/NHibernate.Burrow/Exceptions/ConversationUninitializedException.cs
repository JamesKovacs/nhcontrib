namespace NHibernate.Burrow.Exceptions {
    public class ConversationUninitializedException : DomainException {
        public ConversationUninitializedException()
            : base("Conversation is not yet initialized for this thread yet, Either StartNew or Retrieve first") {}

        public ConversationUninitializedException(string msg) : base(msg) {}
    }
}