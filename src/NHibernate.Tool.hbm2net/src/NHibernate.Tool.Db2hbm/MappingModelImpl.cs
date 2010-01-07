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
            if (propertiesMapByEntity.ContainsKey(q.name))
            {
                q.Items = propertiesMapByEntity[q.name].ToArray();
            }
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
                propertiesMapByEntity[entityName].RemoveAll(q => string.Compare(q.name, p, true) == 0);
            }
        }

        public @class GetClassFromEntityName(string entityName)
        {
            @class ret = null;
            entityMapByEntity.TryGetValue(entityName, out ret);
            return ret;
        }

        #endregion
    }
}
