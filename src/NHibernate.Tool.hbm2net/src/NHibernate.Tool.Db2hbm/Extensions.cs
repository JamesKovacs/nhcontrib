using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    static class Extensions
    {
        public static bool ParseFromDb(this bool boolean,string data)
        {
            data = data.ToLower();
            return data == "yes" || data == "true" || data == "1";
        }

        public static DataTable GetCompleteColumnSchema(this NHibernate.Dialect.Dialect dialect, DbConnection connection, string schema, string table)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = "select * from " + GetTableName(schema,table,dialect);
            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo))
            {
                return reader.GetSchemaTable();
            }
        }
        private static string GetTableName(string schema, string table, NHibernate.Dialect.Dialect dialect)
        {

            if (!string.IsNullOrEmpty(schema))
            {
                return string.Format("{0}.{1}", dialect.QuoteForTableName(schema), dialect.QuoteForTableName(table));
            }
            else
            {
                return dialect.QuoteForTableName(table);
            }
        }
    }
}
