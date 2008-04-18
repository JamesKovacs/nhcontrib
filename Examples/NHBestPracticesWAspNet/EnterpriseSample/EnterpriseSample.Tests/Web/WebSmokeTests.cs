using NUnit.Extensions.Asp;
using NUnit.Framework;

namespace EnterpriseSample.Tests.Web
{
    [TestFixture]
    [Category("Web Smoke Tests")]
    public class WebSmokeTests : WebFormTestCase 
    {
        [Test]
        public void CanLoadListCustomers() {
            Browser.GetPage(TestGlobals.TargetHttpServer + "ListCustomers.aspx");
        }

        [Test]
        public void CanLoadListSuppliers() {
            Browser.GetPage(TestGlobals.TargetHttpServer + "ListSuppliers.aspx");
        }

        [Test]
        public void CanLoadAddCustomer() {
            Browser.GetPage(TestGlobals.TargetHttpServer + "AddCustomer.aspx");
        }

        [Test]
        public void CanLoadEditCustomer() {
            Browser.GetPage(TestGlobals.TargetHttpServer + "EditCustomer.aspx?customerId=" + TestGlobals.TestCustomer.ID);
        }

        [Test]
        public void CanLoadGetCustomerWebService() {
            Browser.GetPage(TestGlobals.TargetHttpServer + "GetCustomer.asmx");
        }
    }
}
