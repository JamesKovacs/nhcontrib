using System;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Core.Dto;
using NUnit.Framework;

namespace EnterpriseSample.Tests.Domain.Dto
{
    [TestFixture]
    public class CustomerDtoTests
    {
        [Test]
        public void CanCreateCustomerDtoWithOrders() {
            Customer customer = new Customer("Acme Anvil");
            new DomainObjectIdSetter<string>().SetIdOf(customer, "ACME");

            Order order1 = new Order(customer);
            order1.OrderDate = new DateTime(2007, 1, 1);
            order1.ShipToName = "Me";

            Order order2 = new Order(customer);
            order2.OrderDate = new DateTime(2007, 2, 2);
            order2.ShipToName = "You";

            Assert.AreEqual(2, customer.Orders.Count);

            CustomerDto customerDto = new CustomerDto(customer);
            Assert.AreEqual("ACME", customerDto.ID);
            Assert.AreEqual("Acme Anvil", customerDto.CompanyName);
            Assert.AreEqual(2, customerDto.OrderDtos.Count);
            Assert.AreEqual("Me", customerDto.OrderDtos[0].ShipToName);
            Assert.AreEqual(new DateTime(2007, 2, 2), customerDto.OrderDtos[1].OrderDate);
        }
    }
}
