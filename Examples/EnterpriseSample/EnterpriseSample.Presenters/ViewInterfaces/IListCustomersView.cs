using System.Collections.Generic;
using EnterpriseSample.Core.Domain;

namespace EnterpriseSample.Presenters.ViewInterfaces
{
    public interface IListCustomersView
    {
        IList<Customer> Customers { set; }
    }
}
