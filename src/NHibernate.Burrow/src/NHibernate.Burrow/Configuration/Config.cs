using System.Collections;
using System.Reflection;
using NHibernate.Burrow.NHDomain;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Burrow.Configuration {
    /// <summary>
    /// This class should be renamed
    /// </summary>
    public class Config {
        /// <summary>
        /// The assembly of the domain. 
        /// </summary>
        public static Assembly CurrentDomainAssembly {
            get { return PersistantUnitRepo.Instance.CurrentDomainAssembly; }
        }

        public static IDictionary DomainAssemblySettings() {
            DomainAssemblySection setting = PersistantUnit.Current.FindAssemblySetting(CurrentDomainAssembly);
            return Util.CopyToDict(setting.DomainAssemblySettings);
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

        #endregion
    }
}