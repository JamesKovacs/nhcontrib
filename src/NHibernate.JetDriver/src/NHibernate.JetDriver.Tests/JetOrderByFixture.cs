using System;
using NHibernate.Criterion;
using NHibernate.JetDriver.Tests.Entities;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NHibernate.JetDriver.Tests
{
    [TestFixture]
    public class JetOrderByFixture : JetTestBase
    {
        [Test]
        public void NHCD33_Select_Statement_With_OrderBy_Should_Run()
        {
            var sql = "SELECT * FROM ACCOUNTS ORDER BY Firstname ASC, Lastname ASC";
            var processedSql = GetTransformedSql(sql);

            Assert.That(processedSql.IndexOf("from", StringComparison.InvariantCultureIgnoreCase), Is.GreaterThan(0));
        }

        [Test]
        public void NHCD33_Select_Statement_Without_OrderBy_Should_Run()
        {
            var sql = "select * from accounts";
            var transformed = GetTransformedSql(sql);

            Assert.That(transformed, Is.EqualTo(sql));            
        }

        [Test]
        public void NHCD36_Can_OrderBy_On_Multiple_Fields()
        {
            using (var s = SessionFactory.OpenSession())
            {
                var criteria = s.CreateCriteria<Foo>()
                                .AddOrder(new Order("LongName", true))
                                .AddOrder(new Order("ShortName", false));

                try
                {
                    var foos = criteria.List<Foo>();
                }
                catch (Exception ex)
                {
                    Assert.Fail("failed to run query", ex.Message);
                }
            }
        }
    }
}