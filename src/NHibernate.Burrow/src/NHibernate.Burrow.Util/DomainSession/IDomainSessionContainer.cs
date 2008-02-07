using System;

namespace NHibernate.Burrow.Util.DomainSession {
    /// <summary>
    /// A Thread Safe DomainSession container which means 
    /// each thread will always get their own domainLayer.
    /// </summary>
    /// 
    public interface IDomainSessionContainer : IDisposable {
        /// <summary>
        /// 
        /// </summary>
        IDomainSession DomainSession { get; set; }
    }
}