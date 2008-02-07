using System;

namespace NHibernate.Burrow.Util.Exceptions {
    public class TypeNotFoundException : Exception {
        public TypeNotFoundException() : base() {}
        public TypeNotFoundException(string msg) : base(msg) {}
    }
}