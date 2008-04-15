using System;

    public class AssertException : Exception
    {
        public AssertException() : base() {}
        public AssertException(string msg) : base(msg) {}
}