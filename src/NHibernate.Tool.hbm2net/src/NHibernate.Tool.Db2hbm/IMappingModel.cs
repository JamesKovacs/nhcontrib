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
    }
}
