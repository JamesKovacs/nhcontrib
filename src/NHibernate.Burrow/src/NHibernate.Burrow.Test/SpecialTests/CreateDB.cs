using NHibernate.Burrow.TestUtil;
using NHibernate.Burrow.Util;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.SpecialTests
{
    [TestFixture]
    public class CreateDB : TestBase
    {
        [Test, Explicit]
        public void CreateTestDataBase()
        {
            new SchemaUtil().CreateSchemas(false, true);
        }
    }
}