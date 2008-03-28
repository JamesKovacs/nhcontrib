using NHibernate.Burrow.TestUtil;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.UtilTests.DomainSession
{
    [TestFixture]
    public class DomainSessionFixture : TestBase {
        [Test, Ignore("DomainSession feature was temporarily disabled")]
        public void InitializationTest() {
            Assert.IsNotNull(BooDomainSession.Current);
        }
    }
}