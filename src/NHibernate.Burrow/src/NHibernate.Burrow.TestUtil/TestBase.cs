using System;
using log4net;
using log4net.Config;
using NHibernate.Burrow.TestUtil.Attributes;
using NHibernate.Burrow.Util;
using NUnit.Framework;

namespace NHibernate.Burrow.TestUtil
{
    public abstract class TestBase
    {
        private const bool OutputDdl = false;
        protected DataProviderBase tdp;
        protected int testYear = DateTime.Now.Year - 5;

        protected virtual bool CleanAndCreateSchema
        {
            get { return false; }
        }

        [SetUp]
        public void Initialize()
        {
            new BurrowFramework().InitWorkSpace(true, null, string.Empty);
            DataProvider dpa = (DataProvider) Attribute.GetCustomAttribute(GetType(), typeof (DataProvider), true);
            if (dpa != null)
            {
                tdp = dpa.CreateDataProvider();
            }
            else
            {
                tdp = CreateDataProvider();
            }
            SetUp();
        }

        [TearDown]
        public void Close()
        {
            TearDown();
            try
            {
                tdp.ClearData();
                new BurrowFramework().CloseWorkSpace();
            }
            finally
            {
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

        protected virtual DataProviderBase CreateDataProvider()
        {
            return new DataProviderBase();
        }

        protected static void CommitAndStartnNewPersistentContext()
        {
            new BurrowFramework().CloseWorkSpace();
            new BurrowFramework().InitWorkSpace();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            if (CleanAndCreateSchema)
            {
                CreateSchema();
            }else {
				new SchemaUtil().UpdateSchemas(false, true);
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            if (CleanAndCreateSchema)
            {
                DropSchema();
            }
        }

        protected void BeginLog()
        {
            XmlConfigurator.Configure();
        }

        protected void CreateSchema()
        {
            new SchemaUtil().CreateSchemas(false, true);
        }

        protected void DropSchema()
        {
            new SchemaUtil().DropSchemas(false, true);
        }
    }
}