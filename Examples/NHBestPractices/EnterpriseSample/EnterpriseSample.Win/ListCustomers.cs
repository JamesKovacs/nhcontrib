using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnterpriseSample.Presenters;
using EnterpriseSample.Presenters.ViewInterfaces;

namespace EnterpriseSample.Win
{
    public partial class ListCustomers : FormBase, IListCustomersView
    {
        private string action;

        public ListCustomers()
            : base()
        {
            InitializeComponent();
        }

        private void ListCustomers_Load(object sender, EventArgs e)
        {
            DisplayMessage();
            InitView();
        }

        private void InitView()
        {
            ListCustomersPresenter presenter = new ListCustomersPresenter(this);
            presenter.InitView();
        }

    private void DisplayMessage() {
        if (action == "updated")
        {
            lblMessage.Text = "The customer was successfully updated.";
        }
        else if (action == "added")
        {
            lblMessage.Text = "The customer was successfully added.";
        }
        else {
            lblMessage.Text = "Click a customer's ID to edit the customer.";
        }
    }

        #region IListCustomersView Members

        public IList<EnterpriseSample.Core.Domain.Customer> Customers
        {
            set { gridCustomers.DataSource = value; }
        }

        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddCustomer window = new AddCustomer();
            window.ShowDialog();
            action = window.ActionResult;
        }

    }
}