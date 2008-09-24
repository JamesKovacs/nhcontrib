using NHibernate.Burrow.Exceptions;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.FrameworkEnvironment
{
    [TestFixture]
    public class ShutDownFixture
    {
        #region Setup/Teardown

        [TearDown]
        public void TearDown()
        {
            if (!f.BurrowEnvironment.IsRunning)
            {
                f.BurrowEnvironment.Start(); //to ensure other tests can run 
            }
        }

        #endregion

        private BurrowFramework f = new BurrowFramework();

        [Test]
        public void CannotInitializeDomainTest()
        {
            f.BurrowEnvironment.ShutDown();
            try
            {
                f.InitWorkSpace();
                Assert.Fail("Failed to throw FrameworkAlreadyShutDownException");
            }
            catch (FrameworkAlreadyShutDownException) {}
        }
    }
}