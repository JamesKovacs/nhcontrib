using System;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// A class where client can obtain managed Nhibernate Session
    /// </summary>
    public interface ISessionManager : IDisposable {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ISession GetSession();

        /// <summary>
        /// 
        /// </summary>
        void CloseSession();

        /// <summary>
        /// 
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// get a un-managed session
        /// </summary>
        /// <returns></returns>
        ISession GetUnManagedSession();
    }
}