using System.IO;
using NHibernate.JetDriver.Tests.Entities;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Collections.Generic;

namespace NHibernate.JetDriver.Tests
{
    [TestFixture]
    public class JetDriverFixture : JetTestBase
    {
        protected override IList<System.Type> EntityTypes
        {
            get { return new[] { typeof(Foo) }; }
        }

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

        [Test]
        public void NHCD25_Supporting_Counter_DataType_For_Identity_Column()
        {
            var tw = new StringWriter();
            new SchemaExport(base.Configuration).Execute(true, false, false, base.SessionFactoryImpl.ConnectionProvider.GetConnection(), tw);
            var schema = tw.ToString();

            Assert.That(schema, Is.Not.Empty);
            Assert.That(schema.Contains("ModuleId  COUNTER"), Is.True, "DataType of ModuleId (PK) is of type COUNTER");
        }
    }
}
