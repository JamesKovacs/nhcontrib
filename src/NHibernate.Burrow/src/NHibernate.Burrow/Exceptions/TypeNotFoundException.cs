using System;

namespace NHibernate.Burrow.Exceptions {
    public class TypeNotFoundException : Exception {
        public TypeNotFoundException() : base() {}
        public TypeNotFoundException(string msg) : base(msg) {}
    }
}