using NHibernate.Burrow.Configuration;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.SpecialTests {
    [TestFixture]
    public class CreateDB {
        [Test, Explicit]
        public void CreateTestDataBase() {
            Config.CreateDatabase();
        }
    }
}