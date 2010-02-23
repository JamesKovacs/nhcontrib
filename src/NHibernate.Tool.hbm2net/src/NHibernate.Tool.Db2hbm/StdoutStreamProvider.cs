using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public class StdoutStreamProvider:IStreamProvider
    {
        #region IStreamProvider Members

        public System.IO.TextWriter GetTextWriter(string entityTable)
        {
            return Console.Out;
        }

        #endregion

        #region IStreamProvider Members


        public void EndWrite()
        {
            
        }

        #endregion
    }
}
