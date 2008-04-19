using System;

namespace NHibernate.Burrow.Exceptions
{
    /// <summary>
    /// Summary description for DomainException.
    /// </summary>
    [Serializable]
    public class BurrowException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        public BurrowException() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public BurrowException(string msg) : base(msg) {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public BurrowException(string msg, Exception innerException) : base(msg, innerException) {}
    }
}