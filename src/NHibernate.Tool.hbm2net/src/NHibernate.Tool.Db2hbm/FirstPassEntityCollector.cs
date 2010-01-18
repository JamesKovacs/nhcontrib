using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect.Schema;
using NHibernate.Tool.hbm2ddl;
using System.Data;
using System.Data.Common;
using log4net;

namespace NHibernate.Tool.Db2hbm
{
    public class FirstPassEntityCollector:IMetadataStrategy
    {
        GenerationContext currentContext;
        const string COLUMN_NAME = "COLUMN_NAME";
        const string INDEX_NAME = "INDEX_NAME";
        const string DATA_TYPE = "DATA_TYPE";
        public ILog logger { set; protected get; }
        public TypeConverter typeConverter { set; protected get; }
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
                string entityName = currentContext.NamingStrategy.EntityNameFromTableName(tableMetaData.Name);
                var clazz = currentContext.Model.AddClassForTable(tableMetaData.Name, entityName);
                if( !string.IsNullOrEmpty(tableMetaData.Schema) )
                    clazz.schema = tableMetaData.Schema;
                AddProperties(clazz,entityName, tableMetaData);
            }
        }

        private void AddProperties(@class clazz,string entity, ITableMetadata tableMetaData)
        {
            //Dictionary<string, bool> columExclusionList = new Dictionary<string,bool>();
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
            int typeOrdinal = columnSet.Columns.IndexOf(DATA_TYPE);
            foreach (DataRow row in columnSet.Rows)
            {
                var cInfo = tableMetaData.GetColumnMetadata(row.ItemArray[nameOrdinal].ToString());
                property p = currentContext.Model.AddPropertyToEntity(entity
                                                                    ,currentContext.NamingStrategy.PropertyNameFromColumnName(cInfo.Name));
                p.notnull = !true.ParseFromDb(cInfo.Nullable);
                p.notnullSpecified = !true.ParseFromDb(cInfo.Nullable);
                p.column = cInfo.Name;
                p.type1 = typeConverter.GetNHType(cInfo);
                if (p.type1 == null)
                {
                    logger.Warn(string.Format("No NHibernate type defined for dbtype:{0} len:{1}", cInfo.TypeName, cInfo.ColumnSize));
                }
                if (cInfo.ColumnSize != 0)
                {
                    p.length = cInfo.ColumnSize.ToString();
                }
                if (cInfo.NumericalPrecision != 0)
                {
                    p.precision = cInfo.NumericalPrecision.ToString();
                }
            }
            
        }

       
        #endregion
    }
}
