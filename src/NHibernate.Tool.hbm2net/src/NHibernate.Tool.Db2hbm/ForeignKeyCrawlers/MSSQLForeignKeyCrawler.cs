using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using NHibernate.Dialect;
using System.Data;

namespace NHibernate.Tool.Db2hbm.ForeignKeyCrawlers
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    class MSSQLForeignKeyCrawlerFactory : IForeignKeyCrawlerFactory
    {
        public void Register()
        {
            var instance = new MSSQLForeignKeyCrawlerFactory();
            ForeignKeyCrawlersRegistar.Register(instance, typeof(MsSql2000Dialect));
            ForeignKeyCrawlersRegistar.Register(instance, typeof(MsSql2005Dialect));
            ForeignKeyCrawlersRegistar.Register(instance, typeof(MsSql2008Dialect));
        }
        #region IForeignKeyCrawlerFactory Members

        public IForeignKeyCrawler Create()
        {
            return new MSSQLForeignKeyCrawler();
        }

        #endregion
    }
    class MSSQLForeignKeyCrawler:IForeignKeyCrawler
    {
        #region IForeignKeyCrawler Members
        public IForeignKeyColumnInfo[] GetForeignKeyColumns(DbConnection dbConnection, string constraintName, string catalog, string schema)
        {
            var shuldNotBeThisWay = dbConnection.CreateCommand();
            shuldNotBeThisWay.CommandType = CommandType.Text;
            //only tested w MSSQL
            shuldNotBeThisWay.CommandText = @"SELECT 
                                KCU1.CONSTRAINT_NAME AS 'FK_CONSTRAINT_NAME'
                                , KCU1.TABLE_NAME AS 'FK_TABLE_NAME'
                                , KCU1.TABLE_CATALOG AS 'FK_TABLE_CATALOG'
                                , KCU1.TABLE_SCHEMA  AS 'FK_TABLE_SCHEMA'
                                , KCU1.COLUMN_NAME AS 'FK_COLUMN_NAME'
                                , KCU1.ORDINAL_POSITION AS 'FK_ORDINAL_POSITION'
                                , KCU2.CONSTRAINT_NAME AS 'UQ_CONSTRAINT_NAME'
                                , KCU2.TABLE_NAME AS 'UQ_TABLE_NAME'
                                , KCU2.TABLE_CATALOG AS 'UQ_TABLE_CATALOG'
                                ,KCU2.TABLE_SCHEMA AS 'UQ_TABLE_SCHEMA'
                                , KCU2.COLUMN_NAME AS 'UQ_COLUMN_NAME'
                                , KCU2.ORDINAL_POSITION AS 'UQ_ORDINAL_POSITION'
                                FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
                                JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1
                                ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG 
                                AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA
                                AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
                                JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2
                                ON KCU2.CONSTRAINT_CATALOG = 
                                RC.UNIQUE_CONSTRAINT_CATALOG 
                                AND KCU2.CONSTRAINT_SCHEMA = 
                                RC.UNIQUE_CONSTRAINT_SCHEMA
                                AND KCU2.CONSTRAINT_NAME = 
                                RC.UNIQUE_CONSTRAINT_NAME
                                AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION AND KCU1.CONSTRAINT_NAME=@p0
                                AND KCU1.CONSTRAINT_CATALOG=@p1
                                AND KCU1.CONSTRAINT_SCHEMA=@p2
                                ";
            var p0 = shuldNotBeThisWay.CreateParameter();
            p0.Value = constraintName;
            p0.ParameterName = "@p0";
            shuldNotBeThisWay.Parameters.Add(p0);
            var p1 = shuldNotBeThisWay.CreateParameter();
            p1.Value = catalog;
            p1.ParameterName = "@p1";
            shuldNotBeThisWay.Parameters.Add(p1);
            var p2 = shuldNotBeThisWay.CreateParameter();
            p2.Value = schema;
            p2.ParameterName = "@p2";
            shuldNotBeThisWay.Parameters.Add(p2);
            var reader = shuldNotBeThisWay.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            List<IForeignKeyColumnInfo> crawled = new List<IForeignKeyColumnInfo>();
            foreach (DataRow dr in dt.Rows)
            {
                crawled.Add(new MSSQLForeignKeyColumnInfo(dr));
            }
            return crawled.ToArray();
        }
        #endregion
    }
    class MSSQLForeignKeyColumnInfo : IForeignKeyColumnInfo
    {
        public MSSQLForeignKeyColumnInfo(DataRow dr)
        {
            ForeignKeyColumnName = Convert.ToString(dr["FK_COLUMN_NAME"]);
            ForeignKeyTableName = Convert.ToString(dr["FK_TABLE_NAME"]);
            ForeignKeyTableSchema = Convert.ToString(dr["FK_TABLE_SCHEMA"]);
            ForeignKeyTableCatalog = Convert.ToString(dr["FK_TABLE_CATALOG"]);
            PrimaryKeyColumnName = Convert.ToString(dr["UQ_COLUMN_NAME"]);
            PrimaryKeyTableName = Convert.ToString(dr["UQ_TABLE_NAME"]);
            PrimaryKeyTableSchema = Convert.ToString(dr["UQ_TABLE_SCHEMA"]);
            PrimaryKeyTableCatalog = Convert.ToString(dr["UQ_TABLE_CATALOG"]);
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
