using System;
using System.Collections.Generic;
using System.Text;
using EnterpriseSample.Core.Domain;

namespace EnterpriseSample.Presenters.ViewInterfaces
{
    public interface IAddCustomerView
    {
        string Message { set; }
        Customer Customer { get; }
    }
}
