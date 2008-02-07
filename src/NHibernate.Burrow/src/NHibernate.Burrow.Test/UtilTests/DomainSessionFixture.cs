using NHibernate.Burrow.TestUtil;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.UtilTests {
    [TestFixture]
    public class DomainSessionFixture : TestBase {
        [Test]
        public void InitializationTest() {
            Assert.IsNotNull(BooDomainSession.Current);
        }
    }
}