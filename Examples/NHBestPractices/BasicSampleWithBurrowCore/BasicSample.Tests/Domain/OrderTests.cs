using BasicSample.Core.Domain;
using NUnit.Framework;

namespace BasicSample.Tests.Domain
{
    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void CanCreateOrder() {
            Customer customer = new Customer("ACME");
            Order order = new Order(customer);

            Assert.IsTrue(customer.Equals(order.OrderedBy));
        }

        [Test]
        public void CanCompareOrders() {
            Order orderA = new Order(new Customer("Acme"));
            Order orderB = new Order(new Customer("Anvil"));

            Assert.AreNotEqual(orderA, null);
            Assert.AreNotEqual(orderA, orderB);

            DomainObjectIdSetter<long> idSetter = new DomainObjectIdSetter<long>();
            idSetter.SetIdOf(orderA, 1);

            Order orderC = new Order(new Customer("Acme"));

            Assert.AreEqual(orderA, orderC);

            idSetter.SetIdOf(orderC, 2);

            Assert.AreNotEqual(orderA, orderC);
        }
    }
}
