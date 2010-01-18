using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NHibernate.Dialect.Schema;
using log4net;

namespace NHibernate.Tool.Db2hbm
{
    class PrimaryKeyStrategy:IMetadataStrategy
    {
        const string ISKEY = "IsKey";
        const string ISIDENTITY = "IsIdentity";
        const string COLNAME = "ColumnName";
        public ILog logger { private get; set; }
        public TypeConverter typeConverter { set; protected get; }
        GenerationContext currentContext;
        #region IMetadataStrategy Members
        public virtual void Process(GenerationContext context)
        {
            currentContext = context;
            foreach (DataRow t in currentContext.Schema.GetTables(null, null, null, new string[0]).Rows)
            {
                IColumnMetadata[] keyColumns = new IColumnMetadata[0];
                var tableMetaData = currentContext.Schema.GetTableMetadata(t, true);
                string entityName = currentContext.NamingStrategy.EntityNameFromTableName(tableMetaData.Name);
                keyColumns = GetKeyColumns(tableMetaData);
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
                id.column1 = keyColumns[0].Name;
                id.type1 =  typeConverter.GetNHType(keyColumns[0]);
                id.name = currentContext.NamingStrategy.PropertyIdNameFromColumnName(keyColumns[0].Name);
                return id;
            }
            else
            if (keyColumns.Length > 1)
            {
                var cid = new compositeid();
                string entityName = currentContext.NamingStrategy.EntityNameFromTableName(tableMetadata.Name);
                cid.@class = currentContext.NamingStrategy.ClassNameForComponentKey(entityName);
                List<keyproperty> keyps = new List<keyproperty>();
                cid.name = currentContext.NamingStrategy.PropertyNameForComponentKey(entityName,cid.@class);
                foreach (IColumnMetadata meta in keyColumns)
                {
                    keyproperty kp = new keyproperty();
                    kp.name = currentContext.NamingStrategy.PropertyNameFromColumnName(meta.Name);
                    kp.column1 = meta.Name;
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
                var dt = currentContext.Dialect.GetCompleteColumnSchema(currentContext.Connection, tableMetaData.Schema, tableMetaData.Name);
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

        private IColumnMetadata[] GetKeyColumns(NHibernate.Dialect.Schema.ITableMetadata tableMetaData)
        {
            if (currentContext.TableExceptions.HasException(tableMetaData.Name, tableMetaData.Catalog, tableMetaData.Schema))
            {
                cfg.db2hbmconfTable exc = currentContext.TableExceptions.GetTableException(tableMetaData.Name, tableMetaData.Catalog, tableMetaData.Schema);
                if (exc.primarykey != null)
                {
                    if (exc.primarykey.keycolumn != null && exc.primarykey.keycolumn.Length > 0)
                    {
                        logger.Info(string.Format("table {0}: using configured primary key def", tableMetaData.Name));
                        try
                        {
                            return exc.primarykey.keycolumn.Select(q => tableMetaData.GetColumnMetadata(q.name)).Where(q=>q!=null).ToArray();
                        }
                        catch (Exception e)
                        {
                            logger.Error("Can't obtain metadata for configured primary key columns.",e);
                            throw e;
                        }
                    }
                }
            }
            //try to found the primary key from the schema
            try
            {
                var dt = currentContext.Dialect.GetCompleteColumnSchema(currentContext.Connection, tableMetaData.Schema, tableMetaData.Name);
                int isKey = dt.Columns.IndexOf(ISKEY);
                int colName = dt.Columns.IndexOf(COLNAME);
                List<IColumnMetadata> cols = new List<IColumnMetadata>();
                foreach (DataRow dr in dt.Rows)
                {
                    if ((bool)dr.ItemArray[isKey])
                    {
                        cols.Add(tableMetaData.GetColumnMetadata(dr.ItemArray[colName].ToString()));
                    }
                }
                return cols.ToArray();
            }
            catch (Exception e)
            {
                logger.Error("Can't obtain primary key metadata from schema.If database does not support key schema information, please consider to use configuration in order to provide keys", e);
                throw e;
            }
        }
        #endregion
    }
}
