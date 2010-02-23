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
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public class ManyToOneStrategy : KeyAwareMetadataStrategy
    {
        GenerationContext currentContext;
        
        #region IMetadataStrategy Members
        protected override void OnProcess(GenerationContext context)
        {
            currentContext = context;
            WireManyToOnes();
        }

        private void WireManyToOnes()
        {
            foreach (string tableToWire in FkForTables.Keys)
            {
                var fks = FkForTables[tableToWire];
                foreach (var manyToOne in fks.Keys)
                {
                    var keyManyToOne = fks[manyToOne];
                    //check if the refferred entity is included
                    string referredTable = keyManyToOne.First().PrimaryKeyTableName;
                    var referredClass = currentContext.Model.GetClassFromTableName(referredTable);
                    var containingClazz = currentContext.Model.GetClassFromTableName(tableToWire);
                    if( null != containingClazz )
                    {
                        logger.Debug(string.Format("{0} working on:", GetType().Name) + containingClazz.table);
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
                            else if (keyManyToOne.Length > 1)
                            {
                                Array.ForEach(keyManyToOne,k=>currentContext.Model.RemovePropertyByColumn(
                                                        containingClazz.name
                                                        ,k.ForeignKeyColumnName)
                                                        );
                                currentContext.Model.AddManyToOneToEntity(containingClazz.name
                                                    , CreateManyToOne(keyManyToOne, referredClass));   
                            }
                            else
                            {
                                logger.Warn("ForeignKey:" + manyToOne + " on table:" + tableToWire + " appear to have 0 columns");
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

        private manytoone CreateManyToOne(IForeignKeyColumnInfo[] keyManyToOne, @class referredClass)
        {
            manytoone mto = new manytoone();
            mto.Items = keyManyToOne.Select(q => new column(){ name=q.ForeignKeyColumnName}).ToArray();
            mto.@class = referredClass.name;
            mto.name = currentContext.NamingStrategy.GetNameForManyToOne(referredClass.name
                                        , keyManyToOne.Select(q=>q.ForeignKeyColumnName).ToArray() );
            return SetIfNullable(mto,keyManyToOne);
        }

        private manytoone CreateManyToOne(IForeignKeyColumnInfo iForeignKeyColumnInfo,@class referredClass)
        {
            manytoone mto = new manytoone();
            mto.column = iForeignKeyColumnInfo.ForeignKeyColumnName;
            mto.@class = referredClass.name;
            mto.name = currentContext.NamingStrategy.GetNameForManyToOne(referredClass.name, new string[] { iForeignKeyColumnInfo.ForeignKeyColumnName });
            return SetIfNullable(mto,new IForeignKeyColumnInfo[]{iForeignKeyColumnInfo});
        }

        private manytoone SetIfNullable(manytoone mto, IForeignKeyColumnInfo[] iForeignKeyColumnInfo)
        {
            bool notnull = true;
            foreach (var fk in iForeignKeyColumnInfo)
            {
                var meta = currentContext.GetTableMetaData(fk.ForeignKeyTableCatalog, fk.ForeignKeyTableSchema, fk.ForeignKeyTableName);
                if( null != meta )
                {
                    var cinfo = meta.GetColumnMetadata(fk.ForeignKeyColumnName);
                    if (true.ParseFromDb(cinfo.Nullable) == true)
                    {
                        notnull = false;
                        break;
                    }
                }
            }
            mto.notnull = notnull;
            mto.notnullSpecified = mto.notnull;
            return mto;
        }
        #endregion
    }
}
