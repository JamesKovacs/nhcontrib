using System;

namespace NHibernate.Burrow {
    /// <summary>
    /// 
    /// </summary>
    public class DomainTemplateException : Exception {
        /// <summary>
        /// 
        /// </summary>
        public DomainTemplateException() : base() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public DomainTemplateException(string msg) : base(msg) {}
    }
}