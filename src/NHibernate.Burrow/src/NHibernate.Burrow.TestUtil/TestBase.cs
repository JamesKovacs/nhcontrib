using System;
using log4net;
using log4net.Config;
using NHibernate.Burrow;
using NHibernate.Burrow.TestUtil.Attributes;
using NHibernate.Burrow.Util;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NHibernate.Burrow.TestUtil {
    public abstract class TestBase {
        protected DataProviderBase tdp;
        protected int testYear = DateTime.Now.Year - 5;
        private const bool OutputDdl = false;
        protected virtual bool CleanAndCreateSchema
        {
            get{ return true; }
        }

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

        protected static void CommitAndStartnNewPersistentContext() {
            Facade.CloseDomain();
            Facade.InitializeDomain();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            if(CleanAndCreateSchema)
            CreateSchema();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            if (CleanAndCreateSchema)
            DropSchema();
        }

        protected void BeginLog() {
            XmlConfigurator.Configure();
        }

        protected void CreateSchema()
        {
         new  SchemaUtil().CreateSchemas();
          
        }

        protected void DropSchema()
        {
            new SchemaUtil().DropSchemas();
          
        }
    }
}