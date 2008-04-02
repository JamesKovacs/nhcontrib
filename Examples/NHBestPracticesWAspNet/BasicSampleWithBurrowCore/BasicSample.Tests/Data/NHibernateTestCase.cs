using BasicSample.Data;
using NHibernate.Burrow;
using NUnit.Framework;

namespace BasicSample.Tests.Data
{
    public class NHibernateTestCase
    { 
        [TestFixtureSetUp]
        public void Setup() {
           Facade.InitializeDomain();
        }

       
        [TestFixtureTearDown]
        public void Dispose() {
            Facade.CloseDomain();
        }
    }
}
