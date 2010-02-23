using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cfg;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public interface ITableExceptions
    {
        db2hbmconfTable GetTableException(string table, string catalog, string schema);
        bool HasException(string table, string catalog, string schema);
    }
}
