using NUnit.Framework;

namespace NHibernate.Burrow.TestWeb.UnitTest
{
    [TestFixture]
    public class RedirectionFixture : TestBase
    {
        [Test]
        public void Test()
        {
            GoTo("Redirection/Default.aspx");
            IE.Button("btnRedirect").Click();
            AssertText("test passed");

        }
    }
}