using System;
using System.Collections.Generic;
using System.Text;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using NHibernate;

namespace EnterpriseSample.Models
{
    public class AddCustomerModel : ModelBase
    {
        private readonly ICustomerDao customerDao;

        public AddCustomerModel()
        {
            customerDao = DaoFactory.GetCustomerDao();
        }

        public string CreateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (customer.ID.Length == 5)
            {
                if (!IsDuplicateOfExisting(customer))
                    customerDao.Save(customer);
                else
                    return
                        "<span style=\"color:red\">The ID you provided is already in use.</span><br />Please change the ID and try again.";
            }
            else
                return
                    "<span style=\"color:red\">The ID you provide must be exactly 5 characters long.</span><br />Please change the ID and try again.";

            return string.Empty;
        }

        /// <summary>
        /// Checks if a customer already exists with the same customer ID.
        /// </summary>
        private bool IsDuplicateOfExisting(Customer customer)
        {
            try
            {
                Customer duplicateCustomer = customerDao.GetById(customer.ID, false);
                return duplicateCustomer != null;
            }
            catch (ObjectNotFoundException)
            {
                return false;
            }
        }


    }
}
