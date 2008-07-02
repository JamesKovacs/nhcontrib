using NUnit.Framework;

namespace NHibernate.Burrow.TestWeb.UnitTest
{
    [TestFixture]
    public class StatefulFieldInDynamicallyLoadControlsTest : TestBase
    {
        [Test]
        public void Test()
        {
            GoTo("StatefulFieldInDynamicallyLoadControls");
            IE.Button("ctl02_btn").Click();
            AssertTestSuccessMessageShown();
        }
    }
}