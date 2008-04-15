using EnterpriseSample.Core.Domain;
using EnterpriseSample.Tests.Domain;

namespace EnterpriseSample.Tests
{
    public class TestGlobals
    {
        public static string SessionFactoryConfigPath {
            get { return @"D:\nhcontrib\trunk\Examples\EnterpriseSample\EnterpriseSample.Web\Config\NorthwindNHibernate.config"; }
        }

        public static string TargetHttpServer {
            get { return "http://localhost:1635/EnterpriseSample.Web/"; } 
        }

        // The following can be uncommented to pre-load all the ASPX pages on the production server
        //public static string TargetHttpServer {
        //    get { return "http://www.productionserver.com/"; }
        //}
        
        /// <summary>
        /// An unchanging customer ID which should always be available within the database for 
        /// running tests against a "real" customer.
        /// </summary>
        public static Customer TestCustomer {
            get {
                Customer customer = new Customer("Cactus Comidas para llevar");
                new DomainObjectIdSetter<string>().SetIdOf(customer, "CACTU");
                customer.ContactName = "Patricio Simpson";
                return customer;
            }
        }
    }
}
