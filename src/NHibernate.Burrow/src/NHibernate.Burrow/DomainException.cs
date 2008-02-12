using System;

namespace NHibernate.Burrow {
    /// <summary>
    /// Summary description for DomainException.
    /// </summary>
    public class DomainException : Exception {
        /// <summary>
        /// 
        /// </summary>
        public DomainException() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public DomainException(string msg) : base(msg) {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public DomainException(string msg, Exception innerException) : base(msg, innerException) {}
    }
}