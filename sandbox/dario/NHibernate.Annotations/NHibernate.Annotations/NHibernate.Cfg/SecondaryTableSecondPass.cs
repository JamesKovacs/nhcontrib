using System.Collections.Generic;
using NHibernate.Annotations.Cfg.Annotations;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Cfg
{
    public class SecondaryTableSecondPass
    {
        private EntityBinder entityBinder;
        private PropertyHolder propertyHolder;
        private System.Type annotatedClass;

        public SecondaryTableSecondPass(EntityBinder entityBinder, PropertyHolder propertyHolder,
                                        System.Type annotatedClass)
        {
            this.entityBinder = entityBinder;
            this.propertyHolder = propertyHolder;
            this.annotatedClass = annotatedClass;
        }

        public void doSecondPass(IDictionary<string, PersistentClass> persistentClasses)
        {
            entityBinder.FinalSecondaryTableBinding(propertyHolder);
        }
    }
}