namespace NHibernate.Burrow.Exceptions {
    public class ConversationUnavailableException : DomainException {
        public ConversationUnavailableException() : base() {}
        public ConversationUnavailableException(string msg) : base(msg) {}
    }
}