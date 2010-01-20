using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    public interface IMappingModel
    {
        @class AddClassForTable(string tableName,string entityName);
        IList<@class> GetEntities();
        property AddPropertyToEntity(string entityName,string propertyName);
        void RemovePropertyByColumn(string entityName, string p);
        @class GetClassFromEntityName(string entityName);
        @class GetClassFromTableName(string tableName);
        manytoone AddManyToOneToEntity(string entityName,manytoone mto);
    }
}
