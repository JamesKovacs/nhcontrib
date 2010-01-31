using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NHibernate.Dialect.Schema;
using log4net;

namespace NHibernate.Tool.Db2hbm
{
    public abstract class KeyAwareMetadataStrategy:IMetadataStrategy
    {
        GenerationContext currentContext;
        Dictionary<string, Dictionary<string, IForeignKeyColumnInfo[]>> fkForTables;
        public ILog logger { protected get; set; }
        const string CONSTRAINT_NAME = "CONSTRAINT_NAME";
        const string ISKEY = "IsKey";
        const string ISIDENTITY = "IsIdentity";
        const string COLNAME = "ColumnName";
        protected IDictionary<string, Dictionary<string, IForeignKeyColumnInfo[]>> FkForTables
        {
            get { return fkForTables; }
        }
        #region IMetadataStrategy Members

        public void Process(GenerationContext context)
        {
            currentContext = context;
            if (context["FKMAP"] == null)
            {
                fkForTables = new Dictionary<string, Dictionary<string, IForeignKeyColumnInfo[]>>();
                try
                {
                    CollectForeignKeysFromSchema();
                }
                catch (Exception e)
                {
                    logger.Warn("Retrieving foreign keys info from schema failed.", e);
                }
                CollectForeignKeyFromConfig();
                context["FKMAP"] = fkForTables;
            }
            else
            {
                fkForTables = context["FKMAP"] as Dictionary<string, Dictionary<string, IForeignKeyColumnInfo[]>>;
            }
            OnProcess(context);
        }

        #endregion

        protected abstract void OnProcess(GenerationContext context);
        protected DataTable GetCompleteColumnSchema(ITableMetadata tableMetaData)
        {
            string key = string.Format("COLUMN.SCHEMA.{0}.{1}.{2}", tableMetaData.Catalog, tableMetaData.Schema, tableMetaData.Name);
            if (null == currentContext[key])
            {
                DataTable dt = currentContext.Dialect.GetCompleteColumnSchema(currentContext.Connection, tableMetaData.Schema, tableMetaData.Name);
                currentContext[key] = dt;
                return dt;
            }
            else
            {
                return currentContext[key] as DataTable;
            }
        }
        protected virtual void CollectForeignKeysFromSchema()
        {
            IForeignKeyCrawler crawler = ForeignKeyCrawlersRegistar.GetForDialect(currentContext.Dialect.GetType());
            foreach (DataRow t in TableEnumerator.GetInstance(currentContext.Schema))
            {
                var tableMeta = currentContext.Schema.GetTableMetadata(t, true);
                var keys = currentContext.Schema.GetForeignKeys(tableMeta.Catalog, tableMeta.Schema, tableMeta.Name);
                int nameIndex = keys.Columns.IndexOf(CONSTRAINT_NAME);
                foreach (DataRow key in keys.Rows)
                {
                    var keyMeta = tableMeta.GetForeignKeyMetadata(key.ItemArray[nameIndex].ToString());
                    var keyColumns = crawler.GetForeignKeyColumns(currentContext.Connection, keyMeta.Name, tableMeta.Catalog, tableMeta.Schema);
                    AddFKColumnInfo(tableMeta.Name, keyMeta.Name, keyColumns);
                }
            }
        }
        private void CollectForeignKeyFromConfig()
        {

            foreach (DataRow t in TableEnumerator.GetInstance(currentContext.Schema))
            {
                var tableMeta = currentContext.Schema.GetTableMetadata(t, true);
                if (currentContext.TableExceptions.HasException(tableMeta.Name, tableMeta.Catalog, tableMeta.Schema))
                {
                    var tablecfg = currentContext.TableExceptions.GetTableException(tableMeta.Name, tableMeta.Catalog, tableMeta.Schema);
                    if (null != tablecfg.foreignkey)
                    {
                        foreach (var fk in tablecfg.foreignkey)
                        {
                            AddFKColumnInfo(tableMeta.Name, fk.constraintname, GetColumnInfoFromConfig(fk, tableMeta));
                        }
                    }
                }
            }
        }
        protected void AddFKColumnInfo(string tablename, string constraint, IForeignKeyColumnInfo[] keyColumns)
        {
            if (!fkForTables.ContainsKey(tablename))
                fkForTables[tablename] = new Dictionary<string, IForeignKeyColumnInfo[]>();
            // need a merge...
            if (!fkForTables[tablename].ContainsKey(constraint))
            {
                fkForTables[tablename][constraint] = keyColumns;
            }
            else
            {
                List<IForeignKeyColumnInfo> orig = new List<IForeignKeyColumnInfo>();
                orig.AddRange(fkForTables[tablename][constraint]);
                orig.AddRange(keyColumns);
                fkForTables[tablename][constraint] = orig.ToArray();
            }
        }
        private IForeignKeyColumnInfo[] GetColumnInfoFromConfig(cfg.db2hbmconfTableForeignkey fk, ITableMetadata metaData)
        {
            List<IForeignKeyColumnInfo> fks = new List<IForeignKeyColumnInfo>();
            foreach (var ci in fk.columnref)
            {
                fks.Add(new ConfiguredForeignKeyColumnInfo(ci, fk, metaData));
            }
            return fks.ToArray();
        }
        protected IColumnMetadata[] GetKeyColumns(NHibernate.Dialect.Schema.ITableMetadata tableMetaData)
        {
            string key = string.Format("PK_{0}.{1}.{2}", tableMetaData.Catalog, tableMetaData.Schema, tableMetaData.Name);
            if (currentContext[key] == null)
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
                                IColumnMetadata[] res = exc.primarykey.keycolumn.Select(q => tableMetaData.GetColumnMetadata(q.name)).Where(q => q != null).ToArray();
                                currentContext[key] = res;
                                return res;
                            }
                            catch (Exception e)
                            {
                                logger.Error("Can't obtain metadata for configured primary key columns.", e);
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
                            var cmeta = tableMetaData.GetColumnMetadata(dr.ItemArray[colName].ToString());
                            if( null != cmeta )
                                cols.Add(cmeta);
                        }
                    }
                    currentContext[key] = cols.ToArray();
                    return currentContext[key] as IColumnMetadata[];
                }
                catch (Exception e)
                {
                    logger.Error("Can't obtain primary key metadata from schema.If database does not support key schema information, please consider to use configuration in order to provide keys", e);
                    throw e;
                }
            }
            else
            {
                return currentContext[key] as IColumnMetadata[];
            }
        }
        class ConfiguredForeignKeyColumnInfo : IForeignKeyColumnInfo
        {
            public ConfiguredForeignKeyColumnInfo(cfg.db2hbmconfTableForeignkeyColumnref cref, cfg.db2hbmconfTableForeignkey fk, ITableMetadata metaData)
            {
                PrimaryKeyTableName = fk.foreigntable;
                PrimaryKeyTableCatalog = fk.foreigncatalog;
                PrimaryKeyTableSchema = fk.foreignschema;
                PrimaryKeyColumnName = cref.foreigncolumn;

                ForeignKeyColumnName = cref.localcolumn;
                ForeignKeyTableCatalog = metaData.Catalog;
                ForeignKeyTableSchema = metaData.Schema;
                ForeignKeyTableSchema = metaData.Name;
            }
            #region IForeignKeyColumnInfo Members

            public string PrimaryKeyColumnName
            {
                get;
                private set;
            }

            public string PrimaryKeyTableName
            {
                get;
                private set;
            }

            public string PrimaryKeyTableSchema
            {
                get;
                private set;
            }

            public string PrimaryKeyTableCatalog
            {
                get;
                private set;
            }

            public string ForeignKeyColumnName
            {
                get;
                private set;
            }

            public string ForeignKeyTableName
            {
                get;
                private set;
            }

            public string ForeignKeyTableSchema
            {
                get;
                private set;
            }

            public string ForeignKeyTableCatalog
            {
                get;
                private set;
            }

            #endregion
        }
    }
}
