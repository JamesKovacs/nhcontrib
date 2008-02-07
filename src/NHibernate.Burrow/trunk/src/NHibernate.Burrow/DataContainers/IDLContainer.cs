using System;

namespace NHibernate.Burrow.DataContainers {
    /// <summary>
    /// A Thread Safe DomainSession container which means 
    /// each thread will always get their own domainLayer.
    /// </summary>
    /// 
    public interface IDLContainer : IDisposable {
        /// <summary>
        /// 
        /// </summary>
        IDomainSession DomainSession { get; set; }
    }
}