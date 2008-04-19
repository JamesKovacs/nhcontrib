using System;
using System.Collections.Generic;
using BasicSample.Core.Utils;

namespace BasicSample.Core.Domain
{
    public class Supplier : DomainObject<long>
    {
        /// <summary>
        /// Needed by ORM for reflective creation.
        /// </summary>
        private Supplier() {}

        public Supplier(string companyName) {
            // Set the public setter instead of the private member directly so that any business 
            // rules will be carried out
            CompanyName = companyName;
        }

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

        public Products Products {
            get {
                if (productsWrapper == null) {
                    productsWrapper = new Products(products);
                }

                return productsWrapper;
            }
        }

        /// <summary>
        /// Hash code should ONLY contain the "business value signature" of the object and not the ID
        /// </summary>
        public override int GetHashCode() {
            return (GetType().FullName + "|" +
                    CompanyName + "|" +
                    ContactName).GetHashCode();
        }

        private string companyName = "";
        private string contactName = "";
        private Products productsWrapper;

        /// <summary>
        /// Populated and maintained by NHibernate
        /// </summary>
        private IList<Product> products = new List<Product>();
    }
}
