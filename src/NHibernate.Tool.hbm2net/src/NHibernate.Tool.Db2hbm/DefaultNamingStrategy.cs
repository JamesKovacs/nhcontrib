using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;

namespace NHibernate.Tool.Db2hbm
{
    class DefaultNamingStrategy:INamingStrategy
    {

        #region INamingStrategy Members

        public string EntityNameFromTableName(string tableName)
        {
            return tableName;
        }

        public string PropertyNameFromColumnName(string columnName)
        {
            return columnName;
        }

        #endregion

        #region INamingStrategy Members


        public string ClassNameForComponentKey(string entityName)
        {
            return entityName + "Key";
        }

        public string PropertyNameForComponentKey(string entityName, string componentClass)
        {
            return "Id";
        }

        #endregion
    }
}
