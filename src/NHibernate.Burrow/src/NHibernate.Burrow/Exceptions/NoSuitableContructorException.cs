using System;

namespace NHibernate.Burrow.Exceptions
{
    public class NoSuitableContructorException : Exception {
        public NoSuitableContructorException() : base() {}
        public NoSuitableContructorException(string msg) : base(msg) {}
    }
}