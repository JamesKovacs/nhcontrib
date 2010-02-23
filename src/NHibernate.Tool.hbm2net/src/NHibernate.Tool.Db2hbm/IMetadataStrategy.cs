using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect.Schema;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public interface IMetadataStrategy
    {
        void Process(GenerationContext context);
    }
}
