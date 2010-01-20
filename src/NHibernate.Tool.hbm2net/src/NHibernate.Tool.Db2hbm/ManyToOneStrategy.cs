using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using NHibernate.Dialect.Schema;
using NHibernate.SqlCommand;
using log4net;

namespace NHibernate.Tool.Db2hbm
{
    public class ManyToOneStrategy:IMetadataStrategy
    {
        public ILog logger { protected get; set; }

        GenerationContext currentContext;
        Dictionary<string, Dictionary<string,IForeignKeyColumnInfo[]>> fkForTables;
        const string CONSTRAINT_NAME = "CONSTRAINT_NAME";
        #region IMetadataStrategy Members
        public void Process(GenerationContext context)
        {
            currentContext = context;
            fkForTables =   new Dictionary<string, Dictionary<string,IForeignKeyColumnInfo[]>>();
            try
            {
                CollectForeignKeysFromSchema();
            }
            catch (Exception e)
            {
                logger.Warn("Retrieving foreign keys info from schema failed.",e);
            }
            CollectForeignKeyFromConfig();
            WireManyToOnes();
        }

        private void WireManyToOnes()
        {
            foreach (string tableToWire in fkForTables.Keys)
            {
                var fks = fkForTables[tableToWire];
                foreach (var manyToOne in fks.Keys)
                {
                    var keyManyToOne = fks[manyToOne];
                    //check if the refferred entity is included
                    string referredTable = keyManyToOne.First().PrimaryKeyTableName;
                    var referredClass = currentContext.Model.GetClassFromTableName(referredTable);
                    var containingClazz = currentContext.Model.GetClassFromTableName(tableToWire);
                    if( null != containingClazz )
                    {
                        if (null == referredClass)
                        {
                            logger.Warn("Foreign key:" + manyToOne + " refer a non icluded table:"+referredTable+". many-to-one ignored ( this is usually a design error)");
                        }
                        else
                        {
                            if (keyManyToOne.Length == 1) // many to one with simple key
                            {
                                currentContext.Model.RemovePropertyByColumn(containingClazz.name,keyManyToOne[0].ForeignKeyColumnName);
                                currentContext.Model.AddManyToOneToEntity(containingClazz.name
                                                    ,CreateManyToOne(keyManyToOne[0],referredClass));   
                            }
                        }
                    }
                    else
                    {
                        logger.Error("Wrong FK definition ( cant find table containing the FK ):"+manyToOne);
                    }
                }
            }
        }

        private manytoone CreateManyToOne(IForeignKeyColumnInfo iForeignKeyColumnInfo,@class referredClass)
        {
            manytoone mto = new manytoone();
            mto.column = iForeignKeyColumnInfo.ForeignKeyColumnName;
            mto.@class = referredClass.name;
            mto.name = currentContext.NamingStrategy.PropertyNameForManyToOne(referredClass.name, new string[] { iForeignKeyColumnInfo.ForeignKeyColumnName });
            return mto;
        }

        private void CollectForeignKeyFromConfig()
        {

            foreach (DataRow t in currentContext.Schema.GetTables(null, null, null, new string[0]).Rows)
            {
                var tableMeta = currentContext.Schema.GetTableMetadata(t, true);
                if (currentContext.TableExceptions.HasException(tableMeta.Name, tableMeta.Catalog, tableMeta.Schema))
                {
                    var tablecfg = currentContext.TableExceptions.GetTableException(tableMeta.Name, tableMeta.Catalog, tableMeta.Schema);
                    foreach (var fk in tablecfg.foreignkey)
                    {
                        AddFKColumnInfo(tableMeta.Name, fk.constraintname, GetColumnInfoFromConfig(fk,tableMeta));
                    }
                }
            }
        }

        private IForeignKeyColumnInfo[] GetColumnInfoFromConfig(cfg.db2hbmconfTableForeignkey fk,ITableMetadata metaData)
        {
            List<IForeignKeyColumnInfo> fks = new List<IForeignKeyColumnInfo>();
            foreach (var ci in fk.columnref)
            {
                fks.Add(new ConfiguredForeignKeyColumnInfo(ci,fk,metaData));
            }
            return fks.ToArray();
        }
        protected virtual void CollectForeignKeysFromSchema()
        {
            IForeignKeyCrawler crawler = ForeignKeyCrawlersRegistar.GetForDialect(currentContext.Dialect.GetType());
            foreach (DataRow t in currentContext.Schema.GetTables(null, null, null, new string[0]).Rows)
            {
                var tableMeta = currentContext.Schema.GetTableMetadata(t,true);
                var keys = currentContext.Schema.GetForeignKeys(tableMeta.Catalog, tableMeta.Schema, tableMeta.Name);
                int nameIndex = keys.Columns.IndexOf(CONSTRAINT_NAME);
                foreach (DataRow key in keys.Rows)
                {
                    var keyMeta = tableMeta.GetForeignKeyMetadata(key.ItemArray[nameIndex].ToString());
                    var keyColumns = crawler.GetForeignKeyColumns(currentContext.Connection, keyMeta.Name, tableMeta.Catalog, tableMeta.Schema);
                    AddFKColumnInfo(tableMeta.Name,keyMeta.Name,keyColumns);
                }
            }
        }

        protected void AddFKColumnInfo(string tablename,string constraint, IForeignKeyColumnInfo[] keyColumns)
        {
            if (!fkForTables.ContainsKey(tablename))
                fkForTables[tablename] = new Dictionary<string, IForeignKeyColumnInfo[]>();
            fkForTables[tablename][constraint] = keyColumns;
        }
        #endregion
        class ConfiguredForeignKeyColumnInfo:IForeignKeyColumnInfo
        {
            public ConfiguredForeignKeyColumnInfo(cfg.db2hbmconfTableForeignkeyColumnref cref,cfg.db2hbmconfTableForeignkey fk,ITableMetadata metaData)
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
