using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public interface IStreamProvider
    {
        TextWriter GetTextWriter(string entityName);
        void EndWrite();
    }
}
