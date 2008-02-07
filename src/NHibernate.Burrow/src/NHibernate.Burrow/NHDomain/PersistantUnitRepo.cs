using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.NHDomain.Exceptions;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// A repository for the perstitant Units
    /// </summary>
    /// <remarks>
    /// repository instances can be retrieved from here
    /// </remarks>
    public class PersistantUnitRepo {
        private static PersistantUnitRepo instance = new PersistantUnitRepo();

        private IList<PersistantUnit> persistantUnits = new List<PersistantUnit>();

        private PersistantUnitRepo() {
            if (DomainContext.Current == null)
                throw new DomainContextUninitializedException(
                    "Facade.InitializeDomain() must be called at the very begining of the application." +
                    "(Also remember to call Facade.CloseDomain() when exit the transaction with application");
            foreach (PUSection pus in MHDomainTemplateSection.GetInstance().PersistantUnits)
                PersistantUnits.Add(new PersistantUnit(pus));
        }

        /// <summary>
        /// The singleton Instance of this class
        /// </summary>
        public static PersistantUnitRepo Instance {
            get { return instance; }
        }

        /// <summary>
        /// All the existing persistant Units in this application
        /// </summary>
        public IList<PersistantUnit> PersistantUnits {
            get { return persistantUnits; }
        }

        /// <summary>
        /// The Persistant Unit the current context is using
        /// </summary>
        /// <remarks>
        /// it will try to found out the domain assembly that is calling this function and search the PU according to that.
        /// If there is only one PU, it will always return that one. 
        /// </remarks>
        public PersistantUnit CurrentPU {
            get {
                if (persistantUnits.Count == 1)
                    return persistantUnits[0];
                return GetPUOfDomainAssembly(CurrentDomainAssembly);
            }
        }

        /// <summary>
        /// The domain assembly the current context is using.
        /// </summary>
        public Assembly CurrentDomainAssembly {
            get {
                if (SingleAssembly != null)
                    return SingleAssembly;

                foreach (StackFrame s in new StackTrace().GetFrames()) {
                    Assembly a = Assembly.GetAssembly(s.GetMethod().DeclaringType);
                    if (VerifyAssemly(a))
                        return a;
                }
                throw new Exception(
                    "There are multilple domain assembly in the config Settings,"
                    + " none of them is callling this");
            }
        }

        /// <summary>
        /// Gets the domainlayer assembley if there is only one domainLayer assembly in all persistant units in this application
        /// </summary>
        /// <return>
        /// return null if there is more than one domain assembly or none
        /// </return>
        private Assembly SingleAssembly {
            get {
                if (PersistantUnits.Count == 1)
                    return PersistantUnits[0].SingleDomainAssembly;
                return null;
            }
        }

        /// <summary>
        /// Gets the PU a particular domain assembly is using.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        internal PersistantUnit GetPUOfDomainAssembly(Assembly assembly) {
            if (PersistantUnits.Count == 1)
                return PersistantUnits[0];
            foreach (PersistantUnit pu in persistantUnits)
                if (pu.DomainLayerAssemblies.Contains(assembly))
                    return pu;
            throw new DomainTemplateException("Persistant Unit cannot be found for " +
                                              assembly.FullName);
        }

        private bool VerifyAssemly(Assembly a) {
            foreach (PersistantUnit unit in persistantUnits)
                if (unit.DomainLayerAssemblies.Contains(a))
                    return true;
            return false;
        }
    }
}