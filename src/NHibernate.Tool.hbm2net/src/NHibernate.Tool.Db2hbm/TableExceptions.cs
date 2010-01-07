using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cfg;

namespace NHibernate.Tool.Db2hbm
{
    class TableExceptions:ITableExceptions
    {
        Dictionary<string, db2hbmconfTable> exceptions = new Dictionary<string, db2hbmconfTable>();
        public TableExceptions(db2hbmconf conf)
        {
            foreach (var k in conf.tables)
            {
                exceptions[GetKey(k.name, k.catalog, k.schema)] = k;
            }
        }
        #region ITableExceptions Members

        public db2hbmconfTable GetTableException(string table, string catalog, string schema)
        {
            return exceptions[GetKey(table, catalog, schema)];
        }

        public bool HasException(string table, string catalog, string schema)
        {
            return exceptions.ContainsKey(GetKey(table, catalog, schema));
        }

        private static string GetKey(string table, string catalog, string schema)
        {
            return string.Join(".", new string[] { catalog ?? "", schema ?? "", table ?? "" }).ToLower();
        }

        #endregion
    }
}
