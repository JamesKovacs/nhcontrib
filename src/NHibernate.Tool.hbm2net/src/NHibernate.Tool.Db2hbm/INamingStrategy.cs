using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    public interface INamingStrategy
    {
        string EntityNameFromTableName(string tableName);
        string PropertyNameFromColumnName(string columnName);
        string PropertyIdNameFromColumnName(string columnName);
        string ClassNameForComponentKey(string entityName);
        string PropertyNameForComponentKey(string entityName,string componentClass);
        string PropertyNameForManyToOne(string referredEntity, string[] columnNames);
    }
}
