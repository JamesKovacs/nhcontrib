using BasicSample.Core.Utils;

namespace BasicSample.Core.Domain
{
    /// <summary>
    /// Encapsulates a historical summary of a particular product ordered.  Note that this isn't a 
    /// <see cref="DomainObject{TId}" />, just a POCO value object.  Furthermore, since it's a value
    /// object, it's immatable after construction.
    /// </summary>
    public class HistoricalOrderSummary
    {
        public HistoricalOrderSummary(string productName, int totalQuantity) {
            Check.Require(totalQuantity >= 0, "totalQuantity must be >= 0 but was " + totalQuantity.ToString());
            Check.Require(!string.IsNullOrEmpty(productName), "productName may not be null or empty");

            this.productName = productName;
            this.totalQuantity = totalQuantity;
        }

        public string ProductName {
            get { return productName; }
        }

        public int TotalQuantity {
            get { return totalQuantity; }
        }

        public override string ToString() {
            return "Product: " + ProductName + "; Quantity: " + TotalQuantity;
        }

        private string productName;
        private int totalQuantity;
    }
}
