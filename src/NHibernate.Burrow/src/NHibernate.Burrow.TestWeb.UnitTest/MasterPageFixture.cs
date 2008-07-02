using NUnit.Framework;

namespace NHibernate.Burrow.TestWeb.UnitTest
{
    [TestFixture]
    public class MasterPageFixture : TestBase
    {
        [Test]
        public void Test()
        {
            GoTo("Materpage");
            IE.Button("ctl00_ContentPlaceHolder1_Button1").Click();
            AssertTestSuccessMessageShown();
        }
    }
}