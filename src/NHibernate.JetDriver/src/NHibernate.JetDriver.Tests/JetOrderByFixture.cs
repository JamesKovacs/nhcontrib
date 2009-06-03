using System;
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
    }
}