using System.Reflection;
using Iesi.Collections.Generic;

namespace NHibernate.Burrow.Impl {
    /// <summary>
    /// A persistant Unit is a unit of a ORM management
    /// </summary>
    /// <remarks>
    /// it consists of a Database, a NHibernate SessionManager, a Nhibernate SessionFactory.
    /// It can be shared by multiple domain layer assemblies 
    /// It's heavy weight
    /// </remarks>
    internal class PersistenceUnit
    {
        private readonly IPersistenceUnitCfg configuration;
        private readonly Cfg.Configuration nHConfiguration;
        private ISessionFactory sessionFactory;
        private readonly SessionManager sessionManager;

        internal PersistenceUnit(IPersistenceUnitCfg cfg)
        {
            configuration = cfg;
            nHConfiguration = CreateNHConfiguration();
            ReBuildSessionfactory();
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
        /// The configuration section that sets this Persistence Unit in the configuration file
        /// </summary>
        /// <remarks>
        /// This class stored the setting information associated with this PU
        /// </remarks>
        public IPersistenceUnitCfg Configuration
        {
            get { return configuration; }
        }

        /// <summary>
        /// the session Manager that is exclusively used within this persistant unit
        /// </summary>
        internal SessionManager SessionManager
        {
            get { return sessionManager; }
        }

        /// <summary>
        /// Rebuild the Session factory
        /// </summary>
        /// <remarks>
        /// in case you need to change the NHConfiguration on the fly
        /// </remarks>
        public void ReBuildSessionfactory()
        {
            sessionFactory = nHConfiguration.BuildSessionFactory();
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