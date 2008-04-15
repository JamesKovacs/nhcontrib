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
        void RegisterInterceptor(IInterceptor interceptor);

        ISession GetSession();

        /// <summary>
        /// Flushes anything left in the session and closes the connection.
        /// </summary>
        void CloseSession();

        ITransaction BeginTransaction();
        void CommitTransaction();
        bool HasOpenTransaction();
        void RollbackTransaction();
    }
}