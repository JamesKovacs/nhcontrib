using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Burrow;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Burrow.Configuration {
    /// <summary>
    /// This class should be renamed
    /// </summary>
    public class Config {
 

      

        /// <summary> 
        /// Creates the specified database. 
        /// </summary> 
        public static void CreateDatabase() {
            foreach (PersistenceUnit pu in PersistenceUnitRepo.Instance.PersistenceUnits) {
                SchemaExport se = new SchemaExport(pu.NHConfiguration);
                se.Create(true, true);
            }
        }

        /// <summary>
        /// Get the DBConnectionString of the Current PersistenceUnit
        /// </summary>
        /// <returns></returns>
        public static string DBConnectionString(System.Type entityType) {
            return
                (string)
                PersistenceUnitRepo.Instance.GetPU(entityType).NHConfiguration.Properties[NHibernate.Cfg.Environment.ConnectionString];
        }

        public static IEnumerable<ISessionFactory> SessionFactories {
            get{ throw new NotImplementedException();}
        }

        #region private methods

        #endregion
    }
}