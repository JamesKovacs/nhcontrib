using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace NHibernate.Tool.Db2hbm
{
    class ApplyEntityExceptionsStrategy:IMetadataStrategy
    {
        public ILog Logger { get; set; }
        public ApplyEntityExceptionsStrategy()
        {
            
        }
        #region IMetadataStrategy Members

        public void Process(GenerationContext context)
        {
            CreateRegex(context);
        }

        private void CreateRegex(GenerationContext context)
        {
            if (null != context.Configuration.entityexceptions)
            {
                foreach (var v in context.Configuration.entityexceptions)
                {
                    
                }
            }
        }

        #endregion
    }
}
