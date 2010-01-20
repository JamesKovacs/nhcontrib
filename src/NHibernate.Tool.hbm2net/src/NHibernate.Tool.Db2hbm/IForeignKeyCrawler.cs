using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace NHibernate.Tool.Db2hbm
{
    public interface IForeignKeyCrawlerFactory
    {
        IForeignKeyCrawler Create();
        void Register();
    }
    public interface IForeignKeyCrawler
    {
        IForeignKeyColumnInfo[] GetForeignKeyColumns(DbConnection dbConnection
                                , string constraintName
                                , string catalog
                                , string schema
                                );
        
    }
}
