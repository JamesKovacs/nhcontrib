using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NHibernate.JetDriver.Tests
{
    [TestFixture]
    public class JetDriverFixture : JetTestBase
    {
        [Test]
        public void NHCD29_Multiple_Tables_In_From_Clause()
        {
            var sqlQuery = "select * from ChildTable c, ParentTable1 t1, ParentTable2 t2 " + 
                              "where c.Parent1ID = t1.ID and c.Parent2ID = c2.ID";

            var transformedQuery = GetTransformedSql(sqlQuery);

            Assert.That(sqlQuery, Is.EqualTo(transformedQuery));
        }

        [Test]
        public void NHCD5_Query_Containing_From_Word()
        {
            var query = "select table.OrganizationalUnitId as orgId, " +
                               "table.AssignId as assignId, "          +
                               "table.ValidFrom as validFrom, "        +
                               "table.ValidTo as validTo, "            + 
                               "table.StatusId as status "             + 
                               "from AssignUnitToStatus table where table.OrganizationalUnitId=?";

            var transformed = GetTransformedSql(query);

            Assert.That(query, Is.EqualTo(transformed));
        }
    }
}
