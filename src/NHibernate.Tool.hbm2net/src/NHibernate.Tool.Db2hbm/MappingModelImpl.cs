using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    class MappingModelImpl:IMappingModel
    {
        Dictionary<string, @class> entityMap = new Dictionary<string, @class>();
        #region IMappingModel Members

        public @class AddClassForTable(string tableName, string entityName)
        {
            @class clazz = new @class();
            clazz.name = entityName;
            clazz.table = tableName;
            entityMap[tableName] = clazz;
            return clazz;
        }

        
        public IList<@class> GetEntities()
        {
            return entityMap.Values.ToArray();
        }

        #endregion
    }
}
