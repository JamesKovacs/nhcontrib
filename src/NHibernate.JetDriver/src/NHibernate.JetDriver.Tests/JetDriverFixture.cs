using System;
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
        public void NHCD23_Supporting_Relative_Path_To_DataSource()
        {
            var relativePath = @"..\data\datafile.mdb";
            var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            var constrRelative = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", relativePath);
            var constrAbsolute = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", absolutePath);

            var connRelative = new JetDbConnection(constrRelative);
            var connAbsolute = new JetDbConnection(constrAbsolute);

            Assert.That(connRelative.ConnectionString.Contains(absolutePath), Is.True);
            Assert.That(connAbsolute.ConnectionString.Contains(absolutePath), Is.True);
        }
    }
}
