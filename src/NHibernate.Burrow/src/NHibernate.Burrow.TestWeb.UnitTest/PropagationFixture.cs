

using NUnit.Framework;

namespace NHibernate.Burrow.TestWeb.UnitTest
{
    [TestFixture]
    public class PropagationFixture : TestBase
    {
        [Test]
        public void Test()
        {
            GoTo("Propagation/Default.aspx");
            IE.Link("BurrowLink1").Click();
            Assert.IsTrue(IE.ContainsText("Test Passed"));
        }
    }
}