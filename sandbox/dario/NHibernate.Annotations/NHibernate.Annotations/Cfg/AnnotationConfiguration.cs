using System;
using System.Collections;
using System.Collections.Generic;
using System.Persistence;
using Iesi.Collections.Generic;
using log4net;
using NHibernate.Annotations.NHibernate.Mapping;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Impl;
using NHibernate.Mapping;
using NHibernate.Util;

namespace NHibernate.Annotations.Cfg
{
    public class AnnotationConfiguration : Configuration
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AnnotationConfiguration));

        public static readonly string ARTEFACT = "hibernate.mapping.precedence";
        public static readonly string DEFAULT_PRECEDENCE = "hbm, class";
        private IDictionary<string, System.Type> annotatedClassEntities;
        private List<System.Type> annotatedClasses;
        private IDictionary<string, AnyMetaDefAttribute> anyMetaDefs;
        private List<CacheHolder> caches;
        private IDictionary<string, AnnotatedClassType> classTypes;
        private ISet<string> defaultNamedGenerators;
        private ISet<string> defaultNamedNativeQueryNames;
        private ISet<string> defaultNamedQueryNames;
        private ISet<string> defaultSqlResulSetMappingNames;
        //private Dictionary<string, Properties> generatorTables;
        //private List<Document> hbmDocuments; //user ordering matters, hence the list
        //private Dictionary<string, Document> hbmEntities;
        private bool inSecondPass;
        private bool isDefaultProcessed;
        private bool isValidatorNotPresentLogged;
        private IDictionary<string, Dictionary<string, Join>> joins;
        private IDictionary<string, string> mappedByResolver;
        private IDictionary<string, IdGenerator> namedGenerators;
        private string precedence;
        private IDictionary<string, string> propertyRefResolver;
        private IDictionary<Table, List<string[]>> tableUniqueConstraints;

        protected internal AnnotationConfiguration(SettingsFactory settingsFactory)
            : base(settingsFactory)
        {
        }

        public AnnotationConfiguration()
        {
        }

        public AnnotationConfiguration AddAnnotatedType<T>() where T : class
        {
            return AddAnnotatedType(typeof(T));
        }

        public AnnotationConfiguration AddAnnotatedType(System.Type persistentClass)
        {
            try
            {
                annotatedClasses.Add(persistentClass);
                return this;
            }
            catch (MappingException me)
            {
                log.Error("Could not compile the mapping annotations", me);
                throw me;
            }
        }

        protected void reset()
        {
            namedGenerators = new Dictionary<string, IdGenerator>();
            joins = new Dictionary<string, Dictionary<string, Join>>();
            classTypes = new Dictionary<string, AnnotatedClassType>();
            //generatorTables = new Dictionary<string, Properties>();
            defaultNamedQueryNames = new HashedSet<string>();
            defaultNamedNativeQueryNames = new HashedSet<string>();
            defaultSqlResulSetMappingNames = new HashedSet<string>();
            defaultNamedGenerators = new HashedSet<string>();
            tableUniqueConstraints = new Dictionary<Table, List<string[]>>();
            mappedByResolver = new Dictionary<string, string>();
            propertyRefResolver = new Dictionary<string, string>();
            annotatedClasses = new List<System.Type>();
            //caches = new ArrayList<CacheHolder>();
            //hbmEntities = new Dictionary<string, Document>();
            annotatedClassEntities = new Dictionary<string, System.Type>();
            //hbmDocuments = new ArrayList<Document>();
            //base.NamingStrategy = EJB3NamingStrategy.INSTANCE; //TODO: esto necesita ser public
            //setEntityResolver( new EJB3DTDEntityResolver() );
            anyMetaDefs = new Dictionary<string, AnyMetaDefAttribute>();
        }

        public ISessionFactory BuildSessionFactory()
        {
            return base.BuildSessionFactory();
        }

        private EventListeners GetInitializedEventListeners()
        {
            throw new System.NotImplementedException();
        }

        protected void SecondPassCompile()
        {
            log.Debug("Execute first pass mapping processing");

            //build annotatedClassEntities
            var tempAnnotatedClasses = new List<System.Type>(annotatedClasses.Count);

            foreach (System.Type clazz in annotatedClasses)
            {
                if (AttributeHelper.IsAttributePresent<EntityAttribute>(clazz))
                {
                    annotatedClassEntities.Add(clazz.Name, clazz);
                    tempAnnotatedClasses.Add(clazz);
                }
                else if (AttributeHelper.IsAttributePresent<MappedSuperclassAttribute>(clazz))
                {
                    tempAnnotatedClasses.Add(clazz);
                }
                //only keep MappedSuperclasses and Entity in this list
            }
            annotatedClasses = tempAnnotatedClasses;

            //process default values first
            if (!isDefaultProcessed)
            {
                AnnotationBinder.BindDefaults(CreateExtendedMappings());
                isDefaultProcessed = true;
            }

            //TODO this should go to a helper code
            //process entities
            if (precedence == null) precedence = Properties[ARTEFACT];
            if (precedence == null) precedence = DEFAULT_PRECEDENCE;
            var precedences = new StringTokenizer(precedence, ",; ", false).GetEnumerator();
            if (precedences.MoveNext())
            {
                throw new MappingException(ARTEFACT + " cannot be empty: " + precedence);
            }
            while (precedences.MoveNext())
            {
                string artifact = precedences.Current;
                RemoveConflictedArtifact(artifact);
                ProcessArtifactsOfType(artifact);
            }

            //int cacheNbr = caches.Count;
            //for (int index = 0; index < cacheNbr; index++)
            //{
            //    CacheHolder cacheHolder = caches[index];
            //    if (cacheHolder.isClass)
            //    {
            //        base.SetCacheConcurrencyStrategy(cacheHolder.role, cacheHolder.usage, cacheHolder.region, cacheHolder.cacheLazy);
            //    }
            //    else
            //    {
            //        base.SetCollectionCacheConcurrencyStrategy(cacheHolder.role, cacheHolder.usage, cacheHolder.region);
            //    }
            //}
            //caches.Clear();

            try
            {
                inSecondPass = true;
                ProcessFkSecondPassInOrder();

                var iter  = secondPasses.GetEnumerator();
                while ( iter.MoveNext() ) 
                {
                    var sp = iter.Current;
				    //do the second pass of fk before the others and remove them
				    if ( sp is CreateKeySecondPass ) 
                    {
					    sp.Invoke(classes);
				    }
			    }

                iter = secondPasses.GetEnumerator();
                while (iter.MoveNext())
                {
                    var sp = iter.Current;
                    //do the SecondaryTable second pass before any association becasue associations can be built on joins
                    if (sp is SecondaryTableSecondPass)
                    {
                        sp.Invoke(classes);
                    }
                }

            }
            catch(RecoverableException ex)
            {
                //TODO: remove then RecoverableException
                //the exception was not recoverable after all
			    throw ex.InnerException;
            }

        }

        private void ProcessFkSecondPassInOrder()
        {
            throw new NotImplementedException();
        }

        private void ProcessArtifactsOfType(string artifact)
        {
            throw new NotImplementedException();
        }

        private void RemoveConflictedArtifact(string artifact)
        {
            throw new NotImplementedException();
        }

        private ExtendedMappings CreateExtendedMappings()
        {
            throw new NotImplementedException();
        }

        /*static*/
        private class CacheHolder
        {
            public CacheHolder(string role, string usage, string region, bool isClass, bool cacheLazy)
            {
                this.role = role;
                this.usage = usage;
                this.region = region;
                this.isClass = isClass;
                this.cacheLazy = cacheLazy;
            }

            public string role;
            public string usage;
            public string region;
            public bool isClass;
            public bool cacheLazy;
        }
    }
}