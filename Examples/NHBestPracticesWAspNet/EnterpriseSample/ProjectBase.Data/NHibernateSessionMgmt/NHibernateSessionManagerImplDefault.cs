using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Web;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;
using ProjectBase.Utils;

namespace ProjectBase.Data
{
    public sealed class NHibernateSessionManagerImplDefault : INHibernateSessionManager
    {
        private static string sessionFactoryConfigPath;

        /// <summary>
        /// Private constructor to enforce singleton
        /// </summary>
        internal NHibernateSessionManagerImplDefault(string sessionFactoryConfigFile)
        {
            sessionFactoryConfigPath = sessionFactoryConfigFile;
        }

        /// <summary>
        /// This method attempts to find a session factory stored in <see cref="sessionFactories" />
        /// via its name; if it can't be found it creates a new one and adds it the hashtable.
        /// </summary>
        private ISessionFactory GetSessionFactory() {
            Check.Require(!string.IsNullOrEmpty(sessionFactoryConfigPath),
                "sessionFactoryConfigPath may not be null nor empty");

            //  Attempt to retrieve a stored SessionFactory from the hashtable.
            ISessionFactory sessionFactory = (ISessionFactory) sessionFactories[sessionFactoryConfigPath];

            //  Failed to find a matching SessionFactory so make a new one.
            if (sessionFactory == null) {
                Check.Require(File.Exists(sessionFactoryConfigPath),
                    "The config file at '" + sessionFactoryConfigPath + "' could not be found");

                Configuration cfg = new Configuration();
                cfg.Configure(sessionFactoryConfigPath);

                //  Now that we have our Configuration object, create a new SessionFactory
                sessionFactory = cfg.BuildSessionFactory();

                if (sessionFactory == null) {
                    throw new InvalidOperationException("cfg.BuildSessionFactory() returned null.");
                }

                sessionFactories.Add(sessionFactoryConfigPath, sessionFactory);
            }

            return sessionFactory;
        }

        /// <summary>
        /// Allows you to register an interceptor on a new session.  This may not be called if there is already
        /// an open session attached to the HttpContext.  If you have an interceptor to be used, modify
        /// the HttpModule to call this before calling BeginTransaction().
        /// </summary>
        public void RegisterInterceptor(IInterceptor interceptor) {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen) {
                throw new CacheException("You cannot register an interceptor once a session has already been opened");
            }

            GetSession(interceptor);
        }

        public ISession GetSession() {
            return GetSession(null);
        }

        /// <summary>
        /// Gets a session with or without an interceptor.  This method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private ISession GetSession(IInterceptor interceptor) {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session == null) {
                if (interceptor != null) {
                    session = GetSessionFactory().OpenSession(interceptor);
                }
                else {
                    session = GetSessionFactory().OpenSession();
                }

                ContextSessions[sessionFactoryConfigPath] = session;
            }

            Check.Ensure(session != null, "session was null");

            return session;
        }

        /// <summary>
        /// Flushes anything left in the session and closes the connection.
        /// </summary>
        public void CloseSession() {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen) {
                //session.Flush();
                session.Close();
            }

            ContextSessions.Remove(sessionFactoryConfigPath);
        }

        public ITransaction BeginTransaction() {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            if (transaction == null) {
                transaction = GetSession().BeginTransaction();
                ContextTransactions.Add(sessionFactoryConfigPath, transaction);
            }

            return transaction;
        }

        public void CommitTransaction() {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            try {
                if (HasOpenTransaction()) {
                    transaction.Commit();
                    ContextTransactions.Remove(sessionFactoryConfigPath);
                }
            }
            catch (HibernateException) {
                RollbackTransaction();
                throw;
            }
        }

        public bool HasOpenTransaction() {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        public void RollbackTransaction() {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            try {
                if (HasOpenTransaction()) {
                    transaction.Rollback();
                }

                ContextTransactions.Remove(sessionFactoryConfigPath);
            }
            finally {
                CloseSession();
            }
        }

        /// <summary>
        /// Since multiple databases may be in use, there may be one transaction per database 
        /// persisted at any one time.  The easiest way to store them is via a hashtable
        /// with the key being tied to session factory.  If within a web context, this uses
        /// <see cref="HttpContext" /> instead of the WinForms specific <see cref="CallContext" />.  
        /// Discussion concerning this found at http://forum.springframework.net/showthread.php?t=572
        /// </summary>
        private Hashtable ContextTransactions {
            get {
                if (IsInWebContext()) {
                    if (HttpContext.Current.Items[TRANSACTION_KEY] == null)
                        HttpContext.Current.Items[TRANSACTION_KEY] = new Hashtable();

                    return (Hashtable)HttpContext.Current.Items[TRANSACTION_KEY];
                }
                else {
                    if (CallContext.GetData(TRANSACTION_KEY) == null)
                        CallContext.SetData(TRANSACTION_KEY, new Hashtable());

                    return (Hashtable)CallContext.GetData(TRANSACTION_KEY);
                }
            }
        }

        /// <summary>
        /// Since multiple databases may be in use, there may be one session per database 
        /// persisted at any one time.  The easiest way to store them is via a hashtable
        /// with the key being tied to session factory.  If within a web context, this uses
        /// <see cref="HttpContext" /> instead of the WinForms specific <see cref="CallContext" />.  
        /// Discussion concerning this found at http://forum.springframework.net/showthread.php?t=572
        /// </summary>
        private Hashtable ContextSessions {
            get {
                if (IsInWebContext()) {
                    if (HttpContext.Current.Items[SESSION_KEY] == null)
                        HttpContext.Current.Items[SESSION_KEY] = new Hashtable();

                    return (Hashtable)HttpContext.Current.Items[SESSION_KEY];
                }
                else {
                    if (CallContext.GetData(SESSION_KEY) == null)
                        CallContext.SetData(SESSION_KEY, new Hashtable());

                    return (Hashtable)CallContext.GetData(SESSION_KEY);
                }
            }
        }

        private bool IsInWebContext() {
            return HttpContext.Current != null;
        }

        private Hashtable sessionFactories = new Hashtable();
        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTIONS";
        private const string SESSION_KEY = "CONTEXT_SESSIONS";
    }
}
