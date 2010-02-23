using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public interface IForeignKeyCrawlerFactory
    {
        IForeignKeyCrawler Create();
        void Register();
    }
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public interface IForeignKeyCrawler
    {
        IForeignKeyColumnInfo[] GetForeignKeyColumns(DbConnection dbConnection
                                , string constraintName
                                , string catalog
                                , string schema
                                );
        
    }
}
