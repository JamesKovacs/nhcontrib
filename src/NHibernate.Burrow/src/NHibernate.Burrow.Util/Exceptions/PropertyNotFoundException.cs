using System;

namespace NHibernate.Burrow.Util.Exceptions {
    public class PropertyNotFoundException : Exception {
        public PropertyNotFoundException() : base() {}
        public PropertyNotFoundException(string msg) : base(msg) {}
    }
}