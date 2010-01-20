using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    public interface IForeignKeyColumnInfo
    {
        string PrimaryKeyColumnName { get; }
        string PrimaryKeyTableName { get; }
        string PrimaryKeyTableSchema { get;  }
        string PrimaryKeyTableCatalog { get;  }

        string ForeignKeyColumnName { get; }
        string ForeignKeyTableName { get; }
        string ForeignKeyTableSchema { get;  }
        string ForeignKeyTableCatalog { get;  }
    }
}
