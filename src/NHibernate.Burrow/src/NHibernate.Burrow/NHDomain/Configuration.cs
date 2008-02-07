using System;
using System.Configuration;
using System.Reflection;
using NHibernate.Burrow.Configuration;
using NHibernate.Burrow.NHDomain.Exceptions;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// This class should be renamed
    /// </summary>
    public class Configuration {
        /// <summary>
        /// The assembly of the domain. 
        /// </summary>
        public static Assembly CurrentDomainAssembly {
            get { return PersistantUnitRepo.Instance.CurrentDomainAssembly; }
        }

        /// <summary>
        /// Indicates if the current Domain Assembly is using a DomainSession
        /// </summary>
        public static bool IsUsingDomainLayer {
            get {
                return
                    !string.IsNullOrEmpty(
                         CurrentDomainSessionFactoryName());
            }
        }

        /// <summary>
        /// find the DomainLayerFactory by using the name set in the key "MHGeneralLib.NHDomain.DomainLayerFactoryName" in the config file
        /// if the key is not set or set to be DEFAULT then return a null;
        /// </summary>
        public static IDomainSessionFactory GetDomainSessionFactory() {
            IDomainSessionFactory factory;
            Assembly currentAssembly = CurrentDomainAssembly;

            string fName = CurrentDomainSessionFactoryName();
            if (string.IsNullOrEmpty(fName))
                throw new Exception(
                    "To use auto managed domainlayer the DomainSessionFactory must be set in the " +
                    " NHibernate.Burrow section in the config file.");

            System.Type fType = currentAssembly.GetType(fName);
            if (fType == null)
                throw new ConfigurationErrorsException("The DomainSessionFactory with the name " + fName +
                                    " was not found in the assembly. Please check domianSessionFactoryName in your DomainTemplate section in the config file. The DomainSession cannot be created.");
            ConstructorInfo ci = fType.GetConstructor(System.Type.EmptyTypes);
            if (ci == null)
                throw new GeneralException(
                    "The DomainSessionFactory must have a public constructor with no paremeters. The DomainSession cannot be created.");
            factory = (IDomainSessionFactory)ci.Invoke(new object[0]);
            return factory;
        }

        /// <summary> 
        /// Creates the specified database. 
        /// </summary> 
        public static void CreateDatabase() {
            foreach (PersistantUnit pu in PersistantUnitRepo.Instance.PersistantUnits) {
                SchemaExport se = new SchemaExport(pu.NHConfiguration);
                se.Create(true, true);
            }
        }

        /// <summary>
        /// Get the DBConnectionString of the Current PersistantUnit
        /// </summary>
        /// <returns></returns>
        public static string DBConnectionString() {
            return
                (string)
                PersistantUnit.Current.Configuration.ORMFrameworkSettingsDict["hibernate.connection.connection_string"];
        }

        #region private methods

        private static string CurrentDomainSessionFactoryName() {
            DomainLayerAssemblyElement setting = PersistantUnit.Current.FindAssemblySetting(CurrentDomainAssembly);
            string retVal = setting.DomianSessionFactoryName;
            if (string.IsNullOrEmpty(retVal)) //this is for backward compatibility issues
                retVal = setting.DomianLayerFactoryName;
            return retVal;
        }

        #endregion
    }
}