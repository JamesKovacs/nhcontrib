using System;

namespace NHibernate.Burrow.Exceptions {
    /// <summary>
    /// 
    /// </summary>
    public class GeneralException : BurrowException {
        /// <summary>
        /// 
        /// </summary>
        public GeneralException() : base() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public GeneralException(string msg) : base(msg) {}
    }
}