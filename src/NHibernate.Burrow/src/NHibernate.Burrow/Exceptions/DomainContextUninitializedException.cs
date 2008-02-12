namespace NHibernate.Burrow.Exceptions {
    public class DomainContextUninitializedException : DomainException {
        public DomainContextUninitializedException() : base() {}
        public DomainContextUninitializedException(string msg) : base(msg) {}
    }
}