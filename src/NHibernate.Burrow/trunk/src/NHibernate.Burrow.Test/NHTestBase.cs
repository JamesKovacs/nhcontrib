using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Burrow.TestUtil;
using NUnit.Framework;
using NHibernate.Burrow.NHDomain;

namespace NHibernate.Burrow.Test
{
    public abstract class NHTestBase
    {
        [SetUp]
        public  void SetUp() {
            DomainContext.Initialize();
            OnSetup();
        }

        [TearDown]
        public  void TearDown() {

            OnTearDown();
             DomainContext.Current.Close();
        }       
  
        
      
 
        protected virtual void OnTearDown() { }
        protected virtual void OnSetup() { }
        protected static string RandomName()
        {
            return RandomStringGenerator.GenerateLetterStrings(5);
        }

        protected static void CommitAndClearSession()
        {
            SessionManager.Flush();

            SessionManager.Instance.GetSession().Clear();

        }

        protected void BeginLog()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
