using NUnit.Framework;

namespace NHibernate.JetDriver.Tests
{
    [TestFixture]
    public class JetDriverFixture : JetTestBase
    {
        [Test]
        public void NHCD29_Column_With_From_Word()
        {
            string sqlQuery = "select t.Name, t.FromDate from TestTable t";

            string transformedQuery = GetTransformedSql(sqlQuery);

            Assert.AreEqual(sqlQuery, transformedQuery);
        }

        [Test]
        public void NHCD29_Multiple_Tables_In_From_Clause()
        {
            string sqlQuery = "select * from ChildTable c, ParentTable1 t1, ParentTable2 t2 " + 
                              "where c.Parent1ID = t1.ID and c.Parent2ID = c2.ID";

            string transformedQuery = GetTransformedSql(sqlQuery);

            Assert.AreEqual(sqlQuery, transformedQuery);
        }

        [Test]
        public void NHCD5_Query_Containing_From_Word()
        {
            string query = "select table.OrganizationalUnitId as orgId, " +
                                  "table.AssignId as assignId, " +
                                  "table.ValidFrom as validFrom, " +
                                  "table.ValidTo as validTo, " + 
                                  "table.StatusId as status " + 
                                  "from AssignUnitToStatus table where table.OrganizationalUnitId=?";

            string transformed = GetTransformedSql(query);

            Assert.AreEqual(query, transformed);
        }
       
    }
}
