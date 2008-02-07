using System;
using System.Reflection;
using log4net;
using log4net.Config;
using NHibernate.Burrow.NHDomain;
using NHibernate.Burrow.TestUtil.Attributes;
using NHibernate.Burrow.TestUtil.Attributes;
using NUnit.Framework;

namespace NHibernate.Burrow.TestUtil {
    public abstract class TestBase {
        protected DataProviderBase tdp;
        protected int testYear = DateTime.Now.Year - 5;

         [SetUp]
        public void Initialize() {
            if(DomainContext.Current!= null)
                DomainContext.Current.Close();
            DomainContext.Initialize();
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
                DomainContext.Current.Close();
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

        
   
    }
}