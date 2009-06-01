using System.Reflection;
using NHibernate.SqlCommand;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NHibernate.JetDriver.Tests
{
    [TestFixture]
    public class JetOrderByFixture 
    {
        [Test]
        public void NHCD33_Select_Statement_With_OrderBy_Should_Run()
        {
            SqlString sql = new SqlString("SELECT * FROM ACCOUNTS ORDER BY Firstname ASC, Lastname ASC");
            JetDriver driver = new JetDriver();

            SqlString processedSql = driver.GetType()
                                           .GetMethod("FinalizeJoins", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod)
                                           .Invoke(driver, new object[] { sql }) as SqlString;

            Assert.That(processedSql, Is.Not.Null);
            Assert.That(processedSql.LastIndexOfCaseInsensitive("FROM"), Is.GreaterThan(0));
        }

        [Test]
        public void NHCD33_Select_Statement_Without_OrderBy_Should_Run()
        {
            SqlString sql = new SqlString("SELECT * FROM ACCOUNTS");
            JetDriver driver = new JetDriver();

            SqlString processedSql = driver.GetType()
                                           .GetMethod("FinalizeJoins", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod)
                                           .Invoke(driver, new object[] { sql }) as SqlString;

            Assert.That(processedSql, Is.Not.Null);
            Assert.That(processedSql.LastIndexOfCaseInsensitive("FROM"), Is.GreaterThan(0));            
        }
    }
}