using BasicSample.Core.Utils;

namespace BasicSample.Core.Domain
{
    public class Product : DomainObject<long>
    {
        /// <summary>
        /// Needed by ORM for reflective creation.
        /// </summary>
        private Product() { }

        public Product(string productName, Supplier suppliedBy) {
            ProductName = productName;
            SuppliedBy = suppliedBy;
        }

        public string ProductName {
            get { return productName; }
            set {
                Check.Require(!string.IsNullOrEmpty(value), "ProductName may not be null or empty");
                productName = value;
            }
        }

        public Supplier SuppliedBy {
            get { return suppliedBy; }
            // Assume that it doesn't make sense to ever change the supplier of a product
            protected set {
                Check.Require(value != null, "SuppliedBy may not be null");
                suppliedBy = value;
            }
        }

        /// <summary>
        /// Hash code should ONLY contain the "business value signature" of the object and not the ID
        /// </summary>
        public override int GetHashCode() {
            return (GetType().FullName + "|" +
                    ProductName + "|" +
                    SuppliedBy.GetHashCode()).GetHashCode();
        }

        private string productName;
        private Supplier suppliedBy;
    }
}
