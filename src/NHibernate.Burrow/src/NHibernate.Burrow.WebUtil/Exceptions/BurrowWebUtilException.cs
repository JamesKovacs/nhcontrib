using System;

namespace NHibernate.Burrow.WebUtil.Exceptions
{
    public class BurrowWebUtilException : Exception
    {
        public BurrowWebUtilException() : base() {}
        public BurrowWebUtilException(string msg) : base(msg) {}
    }
}