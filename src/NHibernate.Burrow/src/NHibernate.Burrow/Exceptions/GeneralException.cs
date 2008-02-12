using System;

namespace NHibernate.Burrow.Exceptions {
    public class GeneralException : Exception {
        public GeneralException() : base() {}
        public GeneralException(string msg) : base(msg) {}
    }
}