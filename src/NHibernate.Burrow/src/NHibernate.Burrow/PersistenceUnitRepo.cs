using System.Collections.Generic;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.Exceptions;
using NHibernate.Engine;

namespace NHibernate.Burrow {
    /// <summary>
    /// A repository for the perstitant Units
    /// </summary>
    /// <remarks>
    /// repository instances can be retrieved from here
    /// </remarks>
    public class PersistenceUnitRepo {
        private static PersistenceUnitRepo instance = new PersistenceUnitRepo();

        private IList<PersistenceUnit> persistenceUnits = new List<PersistenceUnit>();

        private PersistenceUnitRepo() {
            if (DomainContext.Current == null)
                throw new DomainContextUninitializedException(
                    "Facade.InitializeDomain() must be called at the very begining of the application." +
                    "(Also remember to call Facade.CloseDomain() when exit the transaction with application");
            foreach (PersistenceUnitElement pus in NHibernateBurrowCfgSection.GetInstance().PersistenceUnits)
                PersistenceUnits.Add(new PersistenceUnit(pus));
        }

        /// <summary>
        /// The singleton Instance of this class
        /// </summary>
        public static PersistenceUnitRepo Instance {
            get { return instance; }
        }

        /// <summary>
        /// All the existing persistant Units in this application
        /// </summary>
        public IList<PersistenceUnit> PersistenceUnits {
            get { return persistenceUnits; }
        }
         
         
        
        internal PersistenceUnit GetPU(System.Type t) {
            if (PersistenceUnits.Count == 1)
                return PersistenceUnits[0];
            foreach (PersistenceUnit pu in persistenceUnits)
            {    
                ISessionFactoryImplementor sfi = (ISessionFactoryImplementor)pu.SessionFactory;
                if(sfi.GetEntityPersister(t) != null)
                    return pu;
            } 
        
            throw new DomainTemplateException("Persistence Unit cannot be found for " + t);
        }

        public PersistenceUnit GetOnlyPU()
        {
            if (persistenceUnits.Count != 1)
                throw new UnableToGetPersistenceUnitException(
                    "Unable to get persistence unit without an entity type when there are more than one session factories.");
            return persistenceUnits[0];
        }
    }
}