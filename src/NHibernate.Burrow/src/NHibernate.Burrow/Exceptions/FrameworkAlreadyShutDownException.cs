using System;

namespace NHibernate.Burrow.Exceptions {
    public class FrameworkAlreadyShutDownException : BurrowException {
        public FrameworkAlreadyShutDownException() : base() {}
        public FrameworkAlreadyShutDownException(string msg) : base(msg) {}
    }
}