using BasicSample.Core.Domain;
using BasicSample.Tests.Domain;

namespace BasicSample.Tests
{
    public class TestGlobals
    {
        public static string TargetHttpServer {
            get { return "http://localhost/BasicNHibernateSample/"; } 
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
