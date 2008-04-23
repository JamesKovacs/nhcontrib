using System;
using EnterpriseSample.Core.Domain;

/// <summary>
/// Summary description for CustomerEventArgs
/// </summary>
public class CustomerEventArgs : EventArgs
{
    private readonly Customer customer;

    public CustomerEventArgs(Customer customer)
    {
        this.customer = customer;
    }

    public Customer Customer
    { get { return customer; } }
}
