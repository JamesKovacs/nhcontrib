using System.Reflection;
using Iesi.Collections.Generic;
using NHibernate.Burrow.Configuration;

namespace NHibernate.Burrow
{
    /// <summary>
    /// A persistant Unit is a unit of a ORM management
    /// </summary>
    /// <remarks>
    /// it consists of a Database, a NHibernate SessionManager, a Nhibernate SessionFactory.
    /// It can be shared by multiple domain layer assemblies 
    /// It's heavy weight
    /// </remarks>
    public class PersistenceUnit
    {
        private readonly PersistenceUnitElement configuration;
        private readonly ISet<Assembly> domainLayerAssemblies = new HashedSet<Assembly>();
        private readonly IInterceptorFactory interceptorFactory;
        private readonly Cfg.Configuration nHConfiguration;
        private readonly ISessionFactory sessionFactory;
        private readonly SessionManager sessionManager;

        internal PersistenceUnit(PersistenceUnitElement cfg)
        {
            configuration = cfg;

            nHConfiguration = CreateNHConfiguration();
            sessionFactory = nHConfiguration.BuildSessionFactory();
            //Temporarily removed auditLog before we decided whether it should stay in Burrow
            //if (Configuration.EnableAuditLog)
            //    interceptorFactory = AuditLogInterceptorFactory.Instance;
            sessionManager = new SessionManager(this);
        }

        /// <summary>
        /// the name of the PU
        /// </summary>
        /// <remarks>
        /// Set at the configuration File
        /// </remarks>
        public string Name
        {
            get { return configuration.Name; }
        }

        /// <summary>
        /// All the domainLayer assemblies that are using this PU
        /// </summary>
        ///<remarks>
        /// Set at the configuration file
        /// </remarks>
        public ISet<Assembly> DomainLayerAssemblies
        {
            get { return domainLayerAssemblies; }
        }

        /// <summary>
        /// The configuration section that sets this Persistence Unit in the configuration file
        /// </summary>
        /// <remarks>
        /// This class stored the setting information associated with this PU
        /// </remarks>
        public PersistenceUnitElement Configuration
        {
            get { return configuration; }
        }

        /// <summary>
        /// the session Manager that is exclusively used within this persistant unit
        /// </summary>
        public SessionManager SessionManager
        {
            get { return sessionManager; }
        }

        
     

        internal ISessionFactory SessionFactory
        {
            get { return sessionFactory; }
        }

        /// <summary>
        /// The nhibernate configuration of this session Manager
        /// </summary>
        public Cfg.Configuration NHConfiguration
        {
            get { return nHConfiguration; }
        }

        public IInterceptorFactory InterceptorFactory
        {
            get { return interceptorFactory; }
        }

        public static PersistenceUnit Instance(System.Type t)
        {
            return PersistenceUnitRepo.Instance.GetPU(t);
        }

        ///<summary>
        /// Create a NHibernate Configuration
        ///</summary>
        ///<returns></returns>
        private Cfg.Configuration CreateNHConfiguration()
        {
            Cfg.Configuration retVal = new Cfg.Configuration();
            retVal.Configure(configuration.NHConfigFile);
            //retVal.Properties = Configuration.ORMFrameworkSettingsDict;
            //foreach (Assembly assembly in domainLayerAssemblies)
            //    retVal.AddAssembly(assembly);
            return retVal;
        }
    }
}