using System.Data;
using NUnit.Framework;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

namespace NHibernate.JetDriver.Tests
{
    [TestFixture]
    public class JetDriverFixture
    {
        private readonly JetDriver jetDriver = new JetDriver();
        private readonly SqlType[] dummyParameterTypes = { };

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
        
        private string GetTransformedSql(string sqlQuery)
        {
            SqlString sql = SqlString.Parse(sqlQuery);

            IDbCommand command = jetDriver.GenerateCommand(CommandType.Text, sql, dummyParameterTypes);

            return command.CommandText;
        }
    }
}
