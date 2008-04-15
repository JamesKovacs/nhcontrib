
using ProjectBase.Data.NHibernateSessionMgmt;
using ProjectBase.Utils;

namespace ProjectBase.Data
{
    /// <summary>
    /// Handles creation and management of sessions and transactions.  It is a singleton because 
    /// building the initial session factory is very expensive. Inspiration for this class came 
    /// from Chapter 8 of Hibernate in Action by Bauer and King.  Although it is a sealed singleton
    /// you can use TypeMock (http://www.typemock.com) for more flexible testing.
    /// </summary>
    public static class NHibernateSessionManager
    {
        #region Thread-safe, lazy Singleton

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static INHibernateSessionManager Instance
        {
            get
            {
                //Check.Require(sessionFactoryConfigPath != null, "NHibernateSessionManager was not Initialized");
                return Nested.NHibernateSessionManager;
            }
        }

        /// <summary>
        /// Initialize this Manager with a nhibernate's config file.
        /// </summary>
        /// <param name="sessionFactoryConfigFile"></param>
        public static void Init(string sessionFactoryConfigFile)
        {
            sessionFactoryConfigPath = sessionFactoryConfigFile;
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            static Nested() { }

            /*
            internal static readonly INHibernateSessionManager NHibernateSessionManager =
                new NHibernateSessionManagerImplDefault(sessionFactoryConfigPath);
            */

            internal static readonly INHibernateSessionManager NHibernateSessionManager =
                new NHibernateSessionManagerImplBurrow();
        }

        #endregion

        private static string sessionFactoryConfigPath;
    }
}
