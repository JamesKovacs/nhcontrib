using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.TestUtil;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.SpecialTests {
    [TestFixture]
    public class CreateDB : TestBase {
        [Test, Explicit]
        public void CreateTestDataBase() {
            Config.CreateDatabase();
        }
    }
}