using System;

namespace NHibernate.Burrow.TestUtil.Exceptions {
    public class TestUtilException : Exception {
        public TestUtilException() : base() {}
        public TestUtilException(string msg) : base(msg) {}
    }
}