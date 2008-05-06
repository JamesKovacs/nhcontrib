using NUnit.Framework;

namespace NHibernate.Burrow.Test.WorkSpace
{
    [TestFixture]
    public class MultipleWorkSpaceTest
    {
        [Test]
        public void StartOneAfterAnotherTest()
        {
            BurrowFramework bf = new BurrowFramework();
            bf.InitWorkSpace();
            bf.CloseWorkSpace();
            bf.InitWorkSpace();
            bf.CloseWorkSpace();
        }
    }
}