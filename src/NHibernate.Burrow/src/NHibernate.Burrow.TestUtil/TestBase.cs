using System;
using log4net;
using log4net.Config;
using NHibernate.Burrow.NHDomain;
using NHibernate.Burrow.TestUtil.Attributes;
using NUnit.Framework;

namespace NHibernate.Burrow.TestUtil {
    public abstract class TestBase {
        protected DataProviderBase tdp;
        protected int testYear = DateTime.Now.Year - 5;

        [SetUp]
        public void Initialize() {
            Facade.InitializeDomain(true, null);
            DataProvider dpa = (DataProvider) Attribute.GetCustomAttribute(GetType(), typeof (DataProvider), true);
            if (dpa != null)
                tdp = dpa.CreateDataProvider();
            else
                tdp = CreateDataProvider();
            SetUp();
        }

        [TearDown]
        public void Close() {
            TearDown();
            try {
                tdp.ClearData();
                Facade.CloseDomain();
            }
            finally {
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// override to add TearDown logic
        /// </summary>
        protected virtual void TearDown() {}

        /// <summary>
        /// override to add SetUp logic
        /// </summary>
        protected virtual void SetUp() {}

        protected virtual DataProviderBase CreateDataProvider() {
            return new DataProviderBase();
        }

        protected static void CommitAndClearSession() {
            SessionManager.Flush();

            SessionManager.ClearSessions();
        }

        protected void BeginLog() {
            XmlConfigurator.Configure();
        }
    }
}