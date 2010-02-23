using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public interface IMappingModel
    {
        @class AddClassForTable(string tableName,string entityName);
        IList<@class> GetEntities();
        property AddPropertyToEntity(string entityName,string propertyName);
        void RemovePropertyByColumn(string entityName, string p);
        @class GetClassFromEntityName(string entityName);
        @class GetClassFromTableName(string tableName);
        manytoone AddManyToOneToEntity(string entityName,manytoone mto);
        void AddCollectionToEntity(string entityName, object coll);
        object[] GetCollectionsOfEntity(string entityName);
        property[] GetPropertyOfEntity(string entityName);
        manytoone[] GetManyToOnesOfEntity(string entityName);
        void RemoveEntity(string entityName);
        void RemoveCollectionFromEntity(string p, object rem);
        void RemoveProperty(string entityName, property property);
        void RemoveManyToOne(string entityName, manytoone manytoone);
    }
}
