using System;
using System.Reflection;
using Iesi.Collections.Generic;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.NHDomain.AuditLog;
using NHibernate;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// A persistant Unit is a unit of a ORM management
    /// </summary>
    /// <remarks>
    /// it consists of a Database, a NHibernate SessionManager, a Nhibernate SessionFactory.
    /// It can be shared by multiple domain layer assemblies 
    /// It's heavy weight
    /// </remarks>
    public class PersistantUnit {
        private readonly PUSection configuration;
        private readonly ISet<Assembly> domainLayerAssemblies = new HashedSet<Assembly>();
        private readonly NHibernate.Cfg.Configuration nHConfiguration;
        private readonly IInterceptorFactory interceptorFactory;
        private readonly ISessionFactory sessionFactory;
        private readonly SessionManager sessionManager;

        internal PersistantUnit(PUSection cfg) {
            configuration = cfg;
            foreach (DomainLayerAssemblyElement dae in cfg.DomainLayerAssemblies) {
                Assembly a = Assembly.Load(dae.Name);
                if (a == null)
                    throw new DomainTemplateException("Assembly " + dae.Name + " is not found.");
                domainLayerAssemblies.Add(a);
            }
            nHConfiguration = CreateNHConfiguration();
            sessionFactory = nHConfiguration.BuildSessionFactory();
            if (Configuration.EnableAuditLog)
                interceptorFactory = AuditLogInterceptorFactory.Instance;
            sessionManager = new SessionManager(this);
        }

        /// <summary>
        /// the name of the PU
        /// </summary>
        /// <remarks>
        /// Set at the configuration File
        /// </remarks>
        public string Name {
            get { return configuration.Name; }
        }

        /// <summary>
        /// All the domainLayer assemblies that are using this PU
        /// </summary>
        ///<remarks>
        /// Set at the configuration file
        /// </remarks>
        public ISet<Assembly> DomainLayerAssemblies {
            get { return domainLayerAssemblies; }
        }

        /// <summary>
        /// The configuration section that sets this Persistant Unit in the configuration file
        /// </summary>
        /// <remarks>
        /// This class stored the setting information associated with this PU
        /// </remarks>
        public PUSection Configuration {
            get { return configuration; }
        }

        /// <summary>
        /// the session Manager that is exclusively used within this persistant unit
        /// </summary>
        public SessionManager SessionManager {
            get { return sessionManager; }
        }

        /// <summary>
        /// The PU Persistant Unit used by the current context
        /// </summary>
        public static PersistantUnit Current {
            get { return PersistantUnitRepo.Instance.CurrentPU; }
        }

        public static PersistantUnit Instance(System.Type t)
        {
            return PersistantUnitRepo.Instance.GetPUOfDomainAssembly(t.Assembly);
        }
        /// <summary>
        /// Gets the only domain assembly if there is only one
        /// </summary>
        /// <remarks>
        /// return null if there more than one domainAssembly or none
        /// </remarks>
        public Assembly SingleDomainAssembly {
            get {
                if (domainLayerAssemblies.Count == 1)
                    foreach (Assembly assembly in domainLayerAssemblies)
                        return assembly;
                return null;
            }
        }

        internal ISessionFactory SessionFactory {
            get { return sessionFactory; }
        }

        /// <summary>
        /// The nhibernate configuration of this session Manager
        /// </summary>
        public NHibernate.Cfg.Configuration NHConfiguration {
            get { return nHConfiguration; }
        }

        public IInterceptorFactory InterceptorFactory {
            get { return interceptorFactory; }
        }

        /// <summary>
        /// Gets the Assembly Setting for a Assembley 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        /// <remarks>A help method </remarks>
        public DomainLayerAssemblyElement FindAssemblySetting(Assembly a) {
            foreach (DomainLayerAssemblyElement dae in configuration.DomainLayerAssemblies)
                if (dae.Name == a.GetName().Name)
                    return dae;
            throw new DomainTemplateException("Assembly " + a.FullName + " is not found in the section " +
                                              configuration.Name);
        }

        ///<summary>
        /// Create a NHibernate Configuration
        ///</summary>
        ///<returns></returns>
        private NHibernate.Cfg.Configuration CreateNHConfiguration() {
            NHibernate.Cfg.Configuration retVal = new NHibernate.Cfg.Configuration();
            retVal.Properties = Configuration.ORMFrameworkSettingsDict;
            foreach (Assembly assembly in domainLayerAssemblies)
                retVal.AddAssembly(assembly, FindAssemblySetting(assembly).SkipSorting);
            return retVal;
        }
    }
}