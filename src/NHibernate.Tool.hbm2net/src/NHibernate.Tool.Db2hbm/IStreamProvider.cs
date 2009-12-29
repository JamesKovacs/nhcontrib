using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NHibernate.Tool.Db2hbm
{
    public interface IStreamProvider
    {
        TextWriter GetTextWriter(string entityTable, string entitySchema, string entityCatalog);
    }
}
