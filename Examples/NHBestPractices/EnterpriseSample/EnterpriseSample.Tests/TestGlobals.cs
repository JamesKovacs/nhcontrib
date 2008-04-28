using System;
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
            get { return "http://localhost:1460/EnterpriseSample.Web/"; } 
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

                //Add Orders
                Order order1 = new Order(customer);
                order1.OrderDate = DateTime.Today.AddMonths(-1);

                Order order2 = new Order(customer);
                order2.OrderDate = DateTime.Today.AddMonths(-2);
                
                Order order3 = new Order(customer);
                order3.OrderDate = DateTime.Today.AddMonths(-3);

                customer.AddOrder(order1);
                customer.AddOrder(order2);
                customer.AddOrder(order3);

                return customer;
            }
        }
    }
}
