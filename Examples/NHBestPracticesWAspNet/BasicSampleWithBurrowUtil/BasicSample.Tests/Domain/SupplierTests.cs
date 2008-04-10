using BasicSample.Core.Domain;
using NUnit.Framework;

namespace BasicSample.Tests.Domain
{
    [TestFixture]
    public class SupplierTests
    {
        [Test]
        public void CanCreateSupplier() {
            Supplier supplier = new Supplier("ACME");
            supplier.ContactName = "Coyote";

            Assert.AreEqual("ACME", supplier.CompanyName);
            Assert.AreEqual("Coyote", supplier.ContactName);
        }

        [Test]
        public void CanManageSupplierProducts() {
            Supplier supplier = new Supplier("ACME");
            Assert.AreEqual(0, supplier.Products.Count);

            // Would work if the "products" member was publicly visible
            //Assert.AreEqual(0, supplier.products.Count);

            supplier.Products.Add(new Product("Shoe", supplier));
            Assert.AreEqual(1, supplier.Products.Count);
            Assert.AreEqual("Shoe", supplier.Products[0].ProductName);
            Assert.AreEqual(supplier, supplier.Products[0].SuppliedBy);

            // Would work if the "products" member was publicly visible
            //Assert.AreEqual(1, supplier.products.Count);
        }
    }
}
