using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    class MappingModelImpl:IMappingModel
    {
        Dictionary<string, @class> entityMapByTable = new Dictionary<string, @class>();
        Dictionary<string, @class> entityMapByEntity = new Dictionary<string, @class>();
        Dictionary<string, List<property>> propertiesMapByEntity = new Dictionary<string, List<property>>();
        #region IMappingModel Members

        public @class AddClassForTable(string tableName, string entityName)
        {
            @class clazz = new @class();
            clazz.name = entityName;
            clazz.table = tableName;
            entityMapByTable[tableName] = clazz;
            entityMapByEntity[entityName] = clazz;
            return clazz;
        }


        
        public IList<@class> GetEntities()
        {
            return entityMapByTable.Values.Select(q=>Wire(q)).ToArray();
        }

        private @class Wire(@class q)
        {
            List<object> items = new List<object>();
            if (propertiesMapByEntity.ContainsKey(q.name))
            {
                propertiesMapByEntity[q.name].ForEach(i => items.Add(i));
            }
            if (manyToOnesByEntity.ContainsKey(q.name))
            {
                manyToOnesByEntity[q.name].ForEach(i => items.Add(i)); ;
            }
            if (collectionsByEntity.ContainsKey(q.name))
            {
                collectionsByEntity[q.name].ForEach(i => items.Add(i)); ;
            }
            q.Items = items.ToArray();
            return q;
        }

       
        public property AddPropertyToEntity(string entityName, string propertyName)
        {
            property p = new property();
            p.name = propertyName;
            if (!propertiesMapByEntity.ContainsKey(entityName))
                propertiesMapByEntity[entityName] = new List<property>();
            propertiesMapByEntity[entityName].Add(p);
            return p;
        }

        public void RemovePropertyByColumn(string entityName, string p)
        {
            if (propertiesMapByEntity.ContainsKey(entityName))
            {
                propertiesMapByEntity[entityName].RemoveAll(q => string.Compare(q.column==null?q.name:q.column, p, true) == 0);
            }
        }

        public @class GetClassFromEntityName(string entityName)
        {
            @class ret = null;
            entityMapByEntity.TryGetValue(entityName, out ret);
            return ret;
        }

        public @class GetClassFromTableName(string tableName)
        {
            @class ret = null;
            this.entityMapByTable.TryGetValue(tableName, out ret);
            return ret;
        }
        #endregion

        #region IMappingModel Members

        Dictionary<string, List<manytoone>> manyToOnesByEntity = new Dictionary<string, List<manytoone>>();
        public manytoone AddManyToOneToEntity(string entity,manytoone mto)
        {
            if (!manyToOnesByEntity.ContainsKey(entity))
            {
                manyToOnesByEntity[entity] = new List<manytoone>();
            }
            manyToOnesByEntity[entity].Add(mto);
            return mto;
        }
        Dictionary<string, List<object>> collectionsByEntity = new Dictionary<string, List<object>>();
        public void AddCollectionToEntity(string entity, object coll)
        {
            if (!collectionsByEntity.ContainsKey(entity))
            {
                collectionsByEntity[entity] = new List<object>();
            }
            collectionsByEntity[entity].Add(coll);
        }

       
        public property[] GetPropertyOfEntity(string entityName)
        {
            List<property> ret;
            propertiesMapByEntity.TryGetValue(entityName, out ret);
            if (null == ret)
            {
                return new property[0];
            }
            else
            {
                return ret.ToArray();
            }
        }
        public void RemoveEntity(string entityName)
        {
            entityMapByEntity.Remove(entityName);
            var key = entityMapByTable.Where(k => k.Value.name == entityName).FirstOrDefault();
            if( key.Key != null )
                entityMapByTable.Remove(key.Key);
        }
        public object[] GetCollectionsOfEntity(string entityName)
        {
            List<object> list;

            if( collectionsByEntity.TryGetValue(entityName, out list) )
                return list.ToArray();
            return new object[0];
        }

        public void RemoveCollectionFromEntity(string entity, object rem)
        {
            collectionsByEntity[entity].Remove(rem);
        }

        #endregion

        #region IMappingModel Members


        public manytoone[] GetManyToOnesOfEntity(string entityName)
        {
            List<manytoone> list;
            if (manyToOnesByEntity.TryGetValue(entityName, out list))
                return list.ToArray();
            else
                return new manytoone[0];
        }

        public void RemoveProperty(string entityName, property property)
        {
            propertiesMapByEntity[entityName].Remove(property);
        }

        public void RemoveManyToOne(string entityName, manytoone manytoone)
        {
            manyToOnesByEntity[entityName].Remove(manytoone);
        }

        #endregion
    }
}
