using System.Collections.Generic;
using NHibernate.Burrow.Exceptions;
using NHibernate.Engine;

namespace NHibernate.Burrow.Impl
{
    /// <summary>
    /// A repository for the perstitant Units
    /// </summary>
    /// <remarks>
    /// repository instances can be retrieved from here
    /// </remarks>
    internal class PersistenceUnitRepo
    {
        private static PersistenceUnitRepo instance = new PersistenceUnitRepo();

        private IList<PersistenceUnit> persistenceUnits = new List<PersistenceUnit>();

        private PersistenceUnitRepo()
        {
            foreach (
                IPersistenceUnitCfg pus in  new BurrowFramework().BurrowEnvironment.Configuration.PersistenceUnitCfgs)
            {
                PersistenceUnits.Add(new PersistenceUnit(pus));
            }
        }

        /// <summary>
        /// The singleton Instance of this class
        /// </summary>
        public static PersistenceUnitRepo Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// All the existing persistant Units in this application
        /// </summary>
        public IList<PersistenceUnit> PersistenceUnits
        {
            get { return persistenceUnits; }
        }

        internal PersistenceUnit GetPU(System.Type t)
        {
            if (PersistenceUnits.Count == 1)
            {
                return PersistenceUnits[0];
            }
            foreach (PersistenceUnit pu in persistenceUnits)
            {
                ISessionFactoryImplementor sfi = (ISessionFactoryImplementor) pu.SessionFactory;
                if (sfi.GetEntityPersister(t.FullName,false) != null)
                {
                    return pu;
                }
            }

            throw new GeneralException("Persistence Unit cannot be found for " + t);
        }

        public PersistenceUnit GetOnlyPU()
        {
            if (persistenceUnits.Count != 1)
            {
                throw new UnableToGetPersistenceUnitException(
                    "Unable to get persistence unit without an entity type when there are more than one session factories.");
            }
            return persistenceUnits[0];
        }
    }
}