using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NHibernate.Dialect.Schema;
using log4net;

namespace NHibernate.Tool.Db2hbm
{
    class PrimaryKeyStrategy:KeyAwareMetadataStrategy
    {
        const string ISIDENTITY = "IsIdentity";
        const string COLNAME = "ColumnName";

        public TypeConverter typeConverter { set; protected get; }
        GenerationContext currentContext;
        #region IMetadataStrategy Members
        protected override void OnProcess(GenerationContext context)
        {
            currentContext = context;
            foreach (DataRow t in currentContext.Schema.GetTables(null, null, null, new string[0]).Rows)
            {
                IColumnMetadata[] keyColumns = new IColumnMetadata[0];
                var tableMetaData = currentContext.Schema.GetTableMetadata(t, true);
                string entityName = currentContext.NamingStrategy.GetEntityNameFromTableName(tableMetaData.Name);
                keyColumns = GetKeyColumns(tableMetaData);
                logger.Debug("PrimaryKeyStrategy working on:" + tableMetaData.Name);
                // remove key colums for standard properties
                Array.ForEach(keyColumns, q => currentContext.Model.RemovePropertyByColumn(entityName, q.Name));
                @class clazz = currentContext.Model.GetClassFromEntityName(entityName);
                if (null == clazz)
                {
                    logger.Warn(string.Format("Inconsistent meta model:entity {0} does not exists anymore",entityName));
                }
                else
                {
                    clazz.Item = CreateId(keyColumns,tableMetaData);
                }
            }
        }

        protected virtual object CreateId(IColumnMetadata[] keyColumns,ITableMetadata tableMetadata)
        {
            if (keyColumns.Length == 1)
            {
                var id = new id();
                id.generator = GetGenerator(keyColumns[0],tableMetadata);
                id.type1 =  typeConverter.GetNHType(keyColumns[0]);
                id.name = currentContext.NamingStrategy.GetIdPropertyNameFromColumnName(keyColumns[0].Name);
                id.column1 = 0 == string.Compare(id.name, keyColumns[0].Name, true) ? null : keyColumns[0].Name;// keyColumns[0].Name;
                return id;
            }
            else
            if (keyColumns.Length > 1)
            {
                var cid = new compositeid();
                string entityName = currentContext.NamingStrategy.GetEntityNameFromTableName(tableMetadata.Name);
                cid.@class = currentContext.NamingStrategy.GetClassNameForComponentKey(entityName);
                List<keyproperty> keyps = new List<keyproperty>();
                cid.name = currentContext.NamingStrategy.GetNameForComponentKey(entityName,cid.@class);
                foreach (IColumnMetadata meta in keyColumns)
                {
                    keyproperty kp = new keyproperty();
                    kp.name = currentContext.NamingStrategy.GetPropertyNameFromColumnName(meta.Name);
                    kp.column1 = 0 == string.Compare(kp.name, meta.Name, true) ? null : meta.Name;  
                    kp.length = meta.ColumnSize != 0 ? meta.ColumnSize.ToString() : null;
                    kp.type1 = typeConverter.GetNHType(meta);
                    keyps.Add(kp);
                }
                cid.Items = keyps.ToArray();
                return cid;
            }
            return null;
            
        }

        protected virtual generator GetGenerator(IColumnMetadata iColumnMetadata, ITableMetadata tableMetaData)
        {
            if (currentContext.TableExceptions.HasException(tableMetaData.Name, tableMetaData.Catalog, tableMetaData.Schema))
            {
                var exc = currentContext.TableExceptions.GetTableException(tableMetaData.Name, tableMetaData.Catalog, tableMetaData.Schema);
                if (exc.primarykey != null && exc.primarykey.generator != null)
                {
                    generator g = new generator();
                    g.@class = exc.primarykey.generator.@class;
                    List<param> parms = new List<param>();
                    foreach (var p in exc.primarykey.generator.param)
                    {
                        parms.Add(new param() { name=p.name, Text = new string[]{p.Value} });
                    }
                    g.param = parms.ToArray();
                    return g;
                }
            }
            logger.Info(string.Format("Table {0}: trying to infer id generator from schema",tableMetaData.Name));
            try{
                var dt = GetCompleteColumnSchema(tableMetaData);
                int isIdentity = dt.Columns.IndexOf(ISIDENTITY);
                int colName = dt.Columns.IndexOf(COLNAME);
                List<IColumnMetadata> cols = new List<IColumnMetadata>();
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.Compare(dr.ItemArray[colName].ToString(),iColumnMetadata.Name,true) == 0)
                    {
                        if( (bool)dr.ItemArray[isIdentity] )
                            return new generator(){ @class="native" };
                        else
                            return new generator() { @class = "assigned" };
                    }
                }
            }
            catch( Exception e)
            {
                logger.Warn(string.Format("Table {0}: unable to infer id generator from schema. Please consider to use configuration to specify it.", tableMetaData.Name),e);
            }
            return null;
        }

       
        #endregion
    }
}
