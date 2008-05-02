using System;
using System.Collections.Generic;
using System.Text;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;

namespace EnterpriseSample.Models
{
    public class EditCustomerModel : ModelBase
    {
        private Customer customer;
        private readonly ICustomerDao customerDao;

        public EditCustomerModel()
        {
            customerDao = DaoFactory.GetCustomerDao();
        }

        public Customer Customer
        {
            get { return customer; }
        }

        public void setCustomer(string customerId)
        {
            customer = customerDao.GetById(customerId, false);
        }

        public void Update()
        {
            customerDao.SaveOrUpdate(customer);
        }
    }
}
