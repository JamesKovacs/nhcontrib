using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    public class StdoutStreamProvider:IStreamProvider
    {
        #region IStreamProvider Members

        public System.IO.TextWriter GetTextWriter(string entityTable, string entitySchema, string entityCatalog)
        {
            return Console.Out;
        }

        #endregion
    }
}
