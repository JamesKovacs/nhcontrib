namespace BasicThreeTier.Core.Domain
{
    public class Customer
    {
        private string address;
        private string companyName;
        private string contactName;
        private int id;
        private bool isActivated;
        private decimal salesPotential;

        /// <summary>
        /// Needed by ORM for reflective creation.
        /// </summary>
        private Customer()
        {
        }

        public Customer(string contactName)
        {
            ContactName = contactName;
        }

        public int Id
        {
            get { return id; }
            private set { id = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public decimal SalesPotential
        {
            get { return salesPotential; }
            set { salesPotential = value; }
        }

        public bool IsActivated
        {
            get { return isActivated; }
            set { isActivated = value; }
        }

        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        public string ContactName
        {
            get { return contactName; }
            set { contactName = value; }
        } 
    }
}