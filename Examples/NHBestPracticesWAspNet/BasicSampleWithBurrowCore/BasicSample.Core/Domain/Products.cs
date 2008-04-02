using System.Collections;
using System.Collections.Generic;
using BasicSample.Core.Utils;

namespace BasicSample.Core.Domain
{
    public class Products : IEnumerable
    {
        public Products(IList<Product> products) {
            Check.Require(products != null, "products may not be null");

            this.products = products;
        }

        public void Add(Product product) {
            if (product != null && !products.Contains(product)) {
                products.Add(product);
            }
        }

        public int Count {
            get { return products.Count; }
        }

        public IEnumerator GetEnumerator() {
            return products.GetEnumerator();
        }

        public Product this[int i] {
            get { return products[i]; }
        }

        private IList<Product> products;
    }
}
