using System;
using EnterpriseSample.Core.Domain;
using ProjectBase.Utils;

namespace EnterpriseSample.Core.Dto
{
    [Serializable]
    public class OrderDto
    {
        private OrderDto() {}

        public OrderDto(Order order) {
            Check.Require(order != null, "order may not be null");

            orderDate = order.OrderDate;
            shipToName = order.ShipToName;
        }

        public DateTime? OrderDate {
            get { return orderDate; }
            set { orderDate = value; }
        }

        public string ShipToName {
            get { return shipToName; }
            set { shipToName = value; }
        }

        private DateTime? orderDate;
        private string shipToName;
    }
}
