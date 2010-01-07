using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cfg;

namespace NHibernate.Tool.Db2hbm
{
    public interface ITableExceptions
    {
        db2hbmconfTable GetTableException(string table, string catalog, string schema);
        bool HasException(string table, string catalog, string schema);
    }
}
