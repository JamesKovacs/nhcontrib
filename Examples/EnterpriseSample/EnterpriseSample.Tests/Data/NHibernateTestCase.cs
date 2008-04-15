using System.Collections.Specialized;
using NHibernate.Burrow;
using NUnit.Framework;
using ProjectBase.Data;

namespace EnterpriseSample.Tests.Data
{
    public class NHibernateTestCase
    {
        /// <summary>
        /// Initializes the NHibernate session bound to CallContext (since this isn't in an HTTP context).
        /// </summary>
        [TestFixtureSetUp]
        public void Setup() {
            new Facade().InitializeDomain(true, new NameValueCollection());
            NHibernateSessionManager.Init(TestGlobals.SessionFactoryConfigPath);
            NHibernateSessionManager.Instance.BeginTransaction();
        }

        /// <summary>
        /// Properly disposes of the <see cref="NHibernateSessionManager"/>.
        /// This always rolls back the transaction; therefore, changes never get committed.
        /// </summary>
        [TestFixtureTearDown]
        public void Dispose() {
            NHibernateSessionManager.Instance.RollbackTransaction();
            new Facade().CloseDomain();
        }
    }
}
