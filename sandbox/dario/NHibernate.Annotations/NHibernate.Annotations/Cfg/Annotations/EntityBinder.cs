using System;
using System.Collections.Generic;
using System.Persistence;
using log4net;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Annotations.Cfg.Annotations
{
    public class EntityBinder
    {
        private System.Type annotatedClass;
        private int batchSize;
        private string cacheConcurrentStrategy;
        private bool cacheLazyProperty;
        private string cacheRegion;
        private string discriminatorValue = string.Empty ;
        private bool dynamicInsert;
        private bool dynamicUpdate;
        private bool explicitHibernateEntityAnnotation;
        private IDictionary<String, String> filters = new Dictionary<String, String>();
        private bool ignoreIdAnnotations;
        private InheritanceState inheritanceState;
        private bool isPropertyAnnotated = false;
        private bool lazy;
        private ILog log = LogManager.GetLogger(typeof (EntityBinder));
        private ExtendedMappings mappings;
        private String name;
        private OptimisticLockType optimisticLockType;
        private PersistentClass persistentClass;
        private PolymorphismType polymorphismType;
        private String propertyAccessor;
        private System.Type proxyClass;
        private IDictionary<String, Object> secondaryTableJoins = new Dictionary<String, Object>();
        private IDictionary<String, Join> secondaryTables = new Dictionary<String, Join>();
        private bool selectBeforeUpdate;
        private String where;
        
        public Join AddJoin(JoinTable ann, ClassPropertyHolder propertyHolder, bool creation)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, Join> SecondaryTables
        {
            get { return secondaryTables; }
        }

        public void FinalSecondaryTableBinding(PropertyHolder holder)
        {
            throw new NotImplementedException();
        }
    }
}