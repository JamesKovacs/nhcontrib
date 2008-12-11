using NUnit.Framework;

namespace NHibernate.Burrow.TestWeb.UnitTest
{
    [TestFixture]
    public class IgnoreStatefulFieldTest : TestBase
    {
        [Test]
        public void Test()
        {
            GoTo("StateulFieldIgnored");
            IE.Button("btnContinue").Click();
            AssertTestSuccessMessageShown();
        }
    }
}