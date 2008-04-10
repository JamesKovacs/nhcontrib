using BasicSample.Data;
using NHibernate.Burrow;
using NUnit.Framework;

namespace BasicSample.Tests.Data
{
    public class NHibernateTestCase
    { 
        [TestFixtureSetUp]
        public void Setup() {
           new Facade().InitializeDomain();
        }

       
        [TestFixtureTearDown]
        public void Dispose() {
            new Facade().CloseDomain();
        }
    }
}
