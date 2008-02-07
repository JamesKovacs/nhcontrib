using System;

namespace NHibernate.Burrow.NHDomain.Exceptions {
    public class DomainContextUninitializedException : DomainException {
        public DomainContextUninitializedException() : base() {}
        public DomainContextUninitializedException(string msg) : base(msg) {}
    }
}