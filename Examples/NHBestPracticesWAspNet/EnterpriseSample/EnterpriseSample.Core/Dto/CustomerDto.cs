using System;
using System.Collections.Generic;
using EnterpriseSample.Core.Domain;
using ProjectBase.Utils;

namespace EnterpriseSample.Core.Dto
{
    [Serializable]
    public class CustomerDto
    {
        private CustomerDto() {}

        public CustomerDto(Customer customer) {
            Check.Require(customer != null, "customer may not be null");

            id = customer.ID;
            companyName = customer.CompanyName;

            foreach (Order order in customer.Orders) {
                orderDtos.Add(new OrderDto(order));
            }
        }

        public string ID {
            get { return id; }
            set { id = value; }
        }

        public string CompanyName {
            get { return companyName; }
            set { companyName = value; }
        }

        public List<OrderDto> OrderDtos {
            get { return orderDtos; }
            set { orderDtos = value; }
        }

        private List<OrderDto> orderDtos = new List<OrderDto>();
        private string id;
        private string companyName;
    }
}
