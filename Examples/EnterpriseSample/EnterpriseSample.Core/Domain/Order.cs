using System;
using ProjectBase.Utils;

namespace EnterpriseSample.Core.Domain
{
    public class Order : DomainObject<long>
    {
        /// <summary>
        /// This is a placeholder constructor for NHibernate.
        /// A no-argument constructor must be avilable for NHibernate to create the object.
        /// Be sure to call the "primary" constructor so the collections get wired up correctly.
        /// Instead of passing null to the primary constructor, I'd recommend passing a 
        /// "null object": http://www.cs.oberlin.edu/~jwalker/nullObjPattern/.
        /// (But passing null keeps things very simple for the example.)
        /// </summary>
        private Order() {}

        public Order(Customer orderedBy) {
            Check.Require(orderedBy != null, "orderedBy may not be null");

            OrderedBy = orderedBy;
            OrderedBy.AddOrder(this);
        }

        public DateTime? OrderDate {
            get { return orderDate; }
            set { orderDate = value; }
        }

        public string ShipToName {
            get { return shipToName; }
            set { shipToName = value; }
        }

        public Customer OrderedBy {
            get { return orderedBy; }
            protected set { orderedBy = value; }
        }

        public override int GetHashCode() {
            return (GetType().FullName + "|" +
                    ShipToName + "|" +
                    (OrderDate ?? DateTime.MinValue).GetHashCode() + "|" +
                    OrderedBy.GetHashCode()).GetHashCode();
        }

        private Customer orderedBy;
        private DateTime? orderDate;
        private string shipToName = "";
    }
}
