using System;
using NHibernate;

namespace ProjectBase.Data
{
    public interface INHibernateSessionManager
    {
        /// <summary>
        /// Allows you to register an interceptor on a new session.  This may not be called if there is already
        /// an open session attached to the HttpContext.  If you have an interceptor to be used, modify
        /// the HttpModule to call this before calling BeginTransaction().
        /// </summary>
        void RegisterInterceptor(Type type, IInterceptor interceptor);

        ISession GetSession(Type type);

        /// <summary>
        /// Flushes anything left in the session and closes the connection.
        /// </summary>
        void CloseSession(Type type);

        ITransaction BeginTransaction(Type type);
        void CommitTransaction(Type type);
        bool HasOpenTransaction(Type type);
        void RollbackTransaction(Type type);
    }
}