using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    public interface INamingStrategy
    {
        string GetEntityNameFromTableName(string tableName);
        string GetPropertyNameFromColumnName(string columnName);
        string GetIdPropertyNameFromColumnName(string columnName);
        string GetClassNameForComponentKey(string entityName);
        string GetNameForComponentKey(string entityName,string componentClass);
        string GetNameForManyToOne(string referredEntity, string[] columnNames);
        string GetNameForCollection(string collectingClass, int progressive);
        string GetClassNameForCollectionComponent(string collectionTableName);
    }
}
