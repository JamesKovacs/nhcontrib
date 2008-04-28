using System;
using System.Collections.Generic;
using System.Text;
using EnterpriseSample.Core.Domain;

namespace EnterpriseSample.Models
{
    public class EditCustomerModel
    {
        private Customer custumer;

        public Customer Customer
        {
            get { return custumer; }
            set { custumer = value; }
        }
    }
}
