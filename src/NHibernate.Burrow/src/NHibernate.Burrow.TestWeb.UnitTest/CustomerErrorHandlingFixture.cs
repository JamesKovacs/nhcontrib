using NUnit.Framework;

namespace NHibernate.Burrow.TestWeb.UnitTest
{
    [TestFixture]
    public class CustomerErrorHandlingFixture :  TestBase
    {
        [Test]
        public void Test()
        {
            GoTo("CustomErrorHandling");
            IE.Button("btnError").Click();
        }
    }
}