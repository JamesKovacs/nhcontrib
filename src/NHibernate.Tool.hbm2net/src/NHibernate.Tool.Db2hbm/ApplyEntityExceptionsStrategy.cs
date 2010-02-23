using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    class ApplyEntityExceptionsStrategy:IMetadataStrategy
    {
        public ILog Logger { get; set; }
        List<EntityException> exceptions = new List<EntityException>();
        public ApplyEntityExceptionsStrategy()
        {
            
        }
        #region IMetadataStrategy Members

        public void Process(GenerationContext context)
        {
            CreateExceptions(context);
            IList<@class> entities = context.Model.GetEntities();
            foreach (var entity in entities)
            {
                foreach (var exception in exceptions)
                {
                    try
                    {
                        exception.Apply(entity, context.Model);
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Error applying entity exception:" + e.Message);
                    }
                }
            }
        }

        private void CreateExceptions(GenerationContext context)
        {
            if (null != context.Configuration.entityexceptions)
            {
                foreach (var v in context.Configuration.entityexceptions)
                {
                    try
                    {
                        exceptions.Add(new EntityException(v));
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Error creating entity exception", e);
                    }
                }
            }
        }

        #endregion
    }
}
