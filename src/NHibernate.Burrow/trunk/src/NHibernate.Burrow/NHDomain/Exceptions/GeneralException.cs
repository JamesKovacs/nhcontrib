using System;

namespace NHibernate.Burrow.NHDomain.Exceptions {
    public class GeneralException : Exception {
        public GeneralException() : base() {}
        public GeneralException(string msg) : base(msg) {}
    }
}