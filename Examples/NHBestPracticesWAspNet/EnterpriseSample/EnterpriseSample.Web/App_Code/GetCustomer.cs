using System.Collections.Generic;
using System.Web.Services;
using EnterpriseSample.Core.Domain;
using EnterpriseSample.Core.Dto;
using EnterpriseSample.Web;

/// <summary>
/// Summary description for GetCustomer
/// </summary>
[WebService(Namespace = "http://localhost/EnterpriseNHibernateSample")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class GetCustomer : BaseWebService
{
    public GetCustomer() {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /// <summary>
    /// A major problem with integrating NHibernate with web services is that lazily loaded collections
    /// aren't accessible after the result has been sent to the client, since the result is no longer 
    /// tied to the NHibernate session.  As an alternative to returning mapped domain objects, return 
    /// DTOs with all their available properties and collections already initialized.  This adds a 
    /// little work to forcefully load all the applicable collections, but also
    /// sends a "complete" result without having to worry about lazy loading issues.
    /// NOTE:  This should be seen more as an "idea" than a best practice.  I don't have a lot of experience
    /// with web services and so there may be a better approach to this.
    /// </summary>
    [WebMethod]
    public CustomerDto GetCustomerBy(string companyName) {
        if (!string.IsNullOrEmpty(companyName)) {
            Customer exampleCustomer = new Customer(companyName);

            // You could also use GetUniqueByExample, but that would thrown an exception if more than one item matched
            List<Customer> matchingCustomers = DaoFactory.GetCustomerDao().GetByExample(exampleCustomer);

            // Even if more than one customer matched, just return the first result.  This doesn't make much business sense, 
            // but since I'm my own client for developing this example application, I can make it do whatever I want it to do.  ;)
            if (matchingCustomers.Count >= 1) {
                return new CustomerDto(matchingCustomers[0]);
            }
        }

        return null;
    }

}

