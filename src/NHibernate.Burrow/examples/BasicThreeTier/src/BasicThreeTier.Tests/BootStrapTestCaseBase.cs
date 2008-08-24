using NHibernate.Burrow;
using NUnit.Framework;

namespace BasicThreeTier.Tests
{
    /// <summary>
    /// Base of test case with BurrowFramework environment prepared
    /// </summary>
    public class BootStrapTestCaseBase
    {
        private readonly BurrowFramework bf = new BurrowFramework();

        [SetUp]
        public void Setup() {
            bf.InitWorkSpace();
        }

       
        [TearDown]
        public void Dispose() {
            bf.CloseWorkSpace();
        }

        /// <summary>
        /// to mimic ending a request and start a new one
        /// </summary>
        protected void RestartBurrowEnvironment()
        {
            bf.CloseWorkSpace();
            bf.InitWorkSpace();
        }
    }
}