using System;
using System.Collections.Generic;
using EnterpriseSample.Core.DataInterfaces;
using ProjectBase.Utils;

namespace EnterpriseSample.Core.Domain
{
    public class Customer : DomainObject<string>, IHasAssignedId<string>
    {
        #region Constructors

        /// <summary>
        /// Needed by ORM for reflective creation.
        /// </summary>
        private Customer() {}

        public Customer(string companyName) {
            // Set the public setter instead of the private member directly so that any business 
            // rules will be carried out
            CompanyName = companyName;
        }

        #endregion

        #region Properties

        public string CompanyName {
            get { return companyName; }
            set {
                Check.Require(!string.IsNullOrEmpty(value), "A valid company name must be provided");
                companyName = value;
            }
        }

        public string ContactName {
            get { return contactName; }
            set { contactName = value; }
        }

        public IList<Order> Orders {
            get { return new List<Order>(orders).AsReadOnly(); }
            protected set { orders = value; }
        }

        #region Methods

        public void AddOrder(Order order)
        {
            if (order != null && !orders.Contains(order)) {
                orders.Add(order);
            }
        }

        public void RemoveOrder(Order order) {
            if (order != null && orders.Contains(order)) {
                orders.Remove(order);
            }
        }

        #endregion

        public void SetAssignedIdTo(string assignedId) {
            ID = assignedId.Trim().ToUpper();
        }

        /// <summary>
        /// Hash code should ONLY contain the "business value signature" of the object and not the ID
        /// </summary>
        public override int GetHashCode() {
            return (GetType().FullName + "|" +
                    CompanyName + "|" +
                    ContactName).GetHashCode();
        }

        #endregion

        #region Members

        private string companyName = "";
        private string contactName = "";
        private IList<Order> orders = new List<Order>();

        #endregion
    }
}
