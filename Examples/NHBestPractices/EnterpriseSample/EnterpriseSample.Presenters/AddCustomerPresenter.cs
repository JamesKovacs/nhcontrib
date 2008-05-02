using System;
using System.Collections.Generic;
using System.Text;
using EnterpriseSample.Core.DataInterfaces;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Models;
using EnterpriseSample.Presenters.ViewInterfaces;
using ProjectBase.Utils;

namespace EnterpriseSample.Presenters
{
    public class AddCustomerPresenter
    {
        private readonly IAddCustomerView view;
        private readonly AddCustomerModel model;

        public AddCustomerPresenter(IAddCustomerView view)
        {
            Check.Require(view != null, "view may not be null");
            this.view = view;

            model = new AddCustomerModel();
        }

        public bool Create()
        {
            string message = model.CreateCustomer(view.Customer);

            if (string.IsNullOrEmpty(message))
                return true;

            view.Message = message;
            return false;
        }
    }
}
