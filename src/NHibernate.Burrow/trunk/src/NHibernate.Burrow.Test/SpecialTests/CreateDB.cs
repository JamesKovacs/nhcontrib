using NUnit.Framework;

namespace NHibernate.Burrow.Test.SpecialTests {
    [TestFixture]
    public class CreateDB  {
        [Test, Explicit]
        public void CreateTestDataBase(){
            NHibernate.Burrow.NHDomain.Configuration.CreateDatabase();
        }
    }
}