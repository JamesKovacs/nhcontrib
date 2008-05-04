using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;

namespace EnterpriseSample.Win
{
    public partial class AddCustomer : FormBase, IAddCustomerView
    {
        private string actionResult;

        public string ActionResult
        {
            get { return actionResult; }
        }

        public AddCustomer()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            base.InitWorkSpace();

            AddCustomerPresenter presenter = new AddCustomerPresenter(this);
            if (presenter.Create())
            {
                actionResult = "updated";
                Hide();
            }

            base.CloseWorkSpace();
        }

        #region IAddCustomerView Members

        public string Message
        {
            set { lblMessage.Text = value; }
        }

        public Customer Customer
        {
            get
            {
                Customer customer = new Customer(txtCompanyName.Text);
                customer.SetAssignedIdTo(txtCustomerID.Text);
                customer.ContactName = txtContactName.Text;

                return customer;
            }
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            actionResult = "canceled";
            Hide();
        }
    }
}