using System.Data;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

namespace NHibernate.JetDriver.Tests
{
    public abstract class JetTestBase
    {
        private readonly JetDriver jetDriver = new JetDriver();
        private readonly SqlType[] dummyParameterTypes = { };

        protected string GetTransformedSql(string sqlQuery)
        {
            var sql = SqlString.Parse(sqlQuery);
            var command = jetDriver.GenerateCommand(CommandType.Text, sql, dummyParameterTypes);

            return command.CommandText;
        }
    }
}