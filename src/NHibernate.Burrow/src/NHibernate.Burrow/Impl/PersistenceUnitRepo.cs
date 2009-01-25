using System;
using System.Collections.Generic;
using NHibernate.Burrow.Exceptions;
using NHibernate.Engine;

namespace NHibernate.Burrow.Impl {
	/// <summary>
	/// A repository for the perstitant Units
	/// </summary>
	/// <remarks>
	/// repository instances can be retrieved from here
	/// </remarks>
	internal class PersistenceUnitRepo {
		private static PersistenceUnitRepo instance;

		private readonly IList<PersistenceUnit> persistenceUnits = new List<PersistenceUnit>();

		private PersistenceUnitRepo() {}
                
		/// <summary>
		/// The singleton Instance of this class
		/// </summary>
		public static PersistenceUnitRepo Instance {
			get
			{
                Burrow.Impl.FrameworkEnvironment.Instance.ToString(); //ensure Environment
			    return instance;
			}
		}

		/// <summary>
		/// All the existing persistant Units in this application
		/// </summary>
		public IList<PersistenceUnit> PersistenceUnits {
			get { return persistenceUnits; }
		}

		public static void Initialize(IBurrowConfig configuration) {
			instance = new PersistenceUnitRepo();
			foreach (IPersistenceUnitCfg pus in configuration.PersistenceUnitCfgs)
				instance.PersistenceUnits.Add(new PersistenceUnit(pus, configuration.Configurator));
		}

		internal PersistenceUnit GetPU(System.Type t) {
            if(PersistenceUnits.Count == 0)
                throw new PersistenceUnitsNotReadyException();
			if (PersistenceUnits.Count == 1)
				return PersistenceUnits[0];
			foreach (PersistenceUnit pu in persistenceUnits) {
				ISessionFactoryImplementor sfi = (ISessionFactoryImplementor) pu.SessionFactory;
				if (sfi.GetEntityPersister(t.FullName, false) != null)
					return pu;
			}

			throw new GeneralException("Persistence Unit cannot be found for " + t);
		}

		public PersistenceUnit GetPU(string name) {
			foreach (PersistenceUnit unit in persistenceUnits)
				if (unit.Name == name)
					return unit;
			throw new ArgumentException("Cannot find persistant unit  named " + name);
		}

		public PersistenceUnit GetOnlyPU() {
			if (persistenceUnits.Count != 1)
				throw new UnableToGetPersistenceUnitException(
					"Unable to get persistence unit without an entity type when there are more than one session factories.");
			return persistenceUnits[0];
		}

		public static void ResetInstance() {
			instance = null;
		}
	}
}