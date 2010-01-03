using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect.Schema;
using NHibernate.Tool.hbm2ddl;
using System.Data;

namespace NHibernate.Tool.Db2hbm
{
    public class FirstPassEntityCollector:IMetadataStrategy
    {
        #region IMetadataStrategy Members

        public void Process(GenerationContext context)
        {
            
            foreach( DataRow t in context.Schema.GetTables(null,null,null,new string[0]).Rows  )
            {
                var tableMetaData = context.Schema.GetTableMetadata(t, true);
                var clazz = context.Model.AddClassForTable(tableMetaData.Name, tableMetaData.Name);
            }
        }

        #endregion
    }
}
