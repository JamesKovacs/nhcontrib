using System;

namespace NHibernate.Burrow.Exceptions {
    [Serializable]
    public class DomainContextUninitializedException : BurrowException {
        public DomainContextUninitializedException() : base() {}
        public DomainContextUninitializedException(string msg) : base(msg) {}
    }
}