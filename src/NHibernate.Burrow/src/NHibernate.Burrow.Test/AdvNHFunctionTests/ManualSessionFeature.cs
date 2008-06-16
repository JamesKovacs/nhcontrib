using NUnit.Framework;

namespace NHibernate.Burrow.Test.AdvNHFunctionTests
{
    [TestFixture]
    public class ManualSessionFeature 
    {
        [Test]
        public void GetSessionFactoryTest()
        {
            BurrowFramework bf = new BurrowFramework();
            Assert.IsNotNull(bf.GetSessionFactory(typeof (MockEntities.MockEntity)));

        }
    }
}