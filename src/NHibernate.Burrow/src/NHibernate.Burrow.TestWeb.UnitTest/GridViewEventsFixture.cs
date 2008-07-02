using NUnit.Framework;

namespace NHibernate.Burrow.TestWeb.UnitTest
{
    [TestFixture]
    public class GridViewEventsFixture : TestBase
    {
        [Test]
        public void Test()
        {
            GoTo("GridViewEvents/Default.aspx");
            IE.Link("GridView1_ctl02_LinkButton1").Click();
            AssertTestSuccessMessageShown();
        }
    }
}