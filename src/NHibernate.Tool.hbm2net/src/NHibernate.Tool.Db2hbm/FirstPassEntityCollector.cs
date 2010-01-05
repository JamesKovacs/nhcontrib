using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect.Schema;
using NHibernate.Tool.hbm2ddl;
using System.Data;
using System.Data.Common;

namespace NHibernate.Tool.Db2hbm
{
    public class FirstPassEntityCollector:IMetadataStrategy
    {
        GenerationContext currentContext;
        const string COLUMN_NAME = "COLUMN_NAME";
        const string INDEX_NAME = "INDEX_NAME";
        #region IMetadataStrategy Members

        public void Process(GenerationContext context)
        {
            currentContext = context;
            GenerateEntities();
        }

        private void GenerateEntities()
        {
            foreach (DataRow t in currentContext.Schema.GetTables(null, null, null, new string[0]).Rows)
            {
                var tableMetaData = currentContext.Schema.GetTableMetadata(t, true);
                var clazz = currentContext.Model.AddClassForTable(tableMetaData.Name, tableMetaData.Name);
                if( !string.IsNullOrEmpty(tableMetaData.Schema) )
                    clazz.schema = tableMetaData.Schema;
                AddProperties(clazz, tableMetaData);
            }
        }

        private void AddProperties(@class clazz, ITableMetadata tableMetaData)
        {
            Dictionary<string, bool> columExclusionList = new Dictionary<string,bool>();
            /*
            var dt = currentContext.Dialect.GetPrimaryKey(currentContext.Connection, tableMetaData.Schema, tableMetaData.Name);
            
            var indexInfo = currentContext.Schema.GetIndexInfo(tableMetaData.Catalog, tableMetaData.Schema, tableMetaData.Name);
            var foreignKeys = currentContext.Schema.GetForeignKeys(tableMetaData.Catalog, tableMetaData.Schema, tableMetaData.Name);
            int indexName = indexInfo.Columns.IndexOf(INDEX_NAME);
            foreach (DataRow dr in indexInfo.Rows)
            {
                var indexMetadata = tableMetaData.GetIndexMetadata(dr.ItemArray[indexName].ToString());
            }*/
            var columnSet = currentContext.Schema.GetColumns(tableMetaData.Catalog, tableMetaData.Schema, tableMetaData.Name, null);
            int nameOrdinal = columnSet.Columns.IndexOf(COLUMN_NAME);
            List<property> properties = new List<property>();
            foreach (DataRow row in columnSet.Rows)
            {
                var cInfo = tableMetaData.GetColumnMetadata(row.ItemArray[nameOrdinal].ToString());
                property p = new property();
                p.name = cInfo.Name;
                p.notnull = !true.ParseFromDb(cInfo.Nullable);
                p.column = cInfo.Name;
                if (cInfo.ColumnSize != 0)
                {
                    p.length = cInfo.ColumnSize.ToString();
                }
                if (cInfo.NumericalPrecision != 0)
                {
                    p.precision = cInfo.NumericalPrecision.ToString();
                }
                properties.Add(p);
            }
            clazz.Items = properties.ToArray();
        }
        #endregion
    }
}
