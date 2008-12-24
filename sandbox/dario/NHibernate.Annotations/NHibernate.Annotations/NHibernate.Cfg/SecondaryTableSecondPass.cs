using System.Collections.Generic;
using NHibernate.Annotations.Cfg.Annotations;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Cfg
{
    public class SecondaryTableSecondPass : ISecondPass
    {
        private EntityBinder entityBinder;
        private IPropertyHolder propertyHolder;
        private System.Type annotatedClass;

        public SecondaryTableSecondPass(EntityBinder entityBinder, IPropertyHolder propertyHolder,
                                        System.Type annotatedClass)
        {
            this.entityBinder = entityBinder;
            this.propertyHolder = propertyHolder;
            this.annotatedClass = annotatedClass;
        }

        public void DoSecondPass(IDictionary<string, PersistentClass> persistentClasses)
        {
            entityBinder.FinalSecondaryTableBinding(propertyHolder);
        }
    }
}