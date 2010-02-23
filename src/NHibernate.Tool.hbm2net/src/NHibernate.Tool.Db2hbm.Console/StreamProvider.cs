using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NHibernate.Tool.Db2hbm.Console
{
    class StreamProvider:IStreamProvider,IDisposable
    {
        string outPath;
        public StreamProvider(string outPath)
        {
            this.outPath = outPath;
            
        }
        #region IStreamProvider Members
        Stream current;
        public System.IO.TextWriter GetTextWriter(string entityName)
        {
            current = new FileStream(Path.Combine(outPath, entityName + ".hbm.xml"),FileMode.Create,FileAccess.ReadWrite);
            return new StreamWriter(current);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        public void EndWrite()
        {
            current.Close();
        }

        #endregion
    }
}
