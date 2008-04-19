using System.Collections.Generic;
using System.Configuration;

namespace NHibernate.Burrow
{
    public interface IBurrowConfig
    {
        ///<summary>
        /// The converstaion timout minutes
        ///</summary>
        int ConversationTimeOut { get; set; }

        ///<summary>
        /// The conversation clean up frequency,
        ///  for how many times of conversation Timeout,
        ///  the conversation pool clean up its timeoutted converstaions.
        ///  must be greater than 1
        ///</summary>
        int ConversationCleanupFrequency { get; set; }

        ///<summary>
        /// The conversation clean up frequency,
        ///  for how many times of conversation Timeout,
        ///  the conversation pool clean up its timeoutted converstaions.
        ///  must be greater than 1
        ///</summary>
        string ConversationExpirationChecker { get; set; }

        ICollection<IPersistenceUnitCfg> PersistenceUnitCfgs { get; }

        /// <summary>
        /// for user to set a customer IWorkSpaceNameSniffer for WebUtil to use
        /// </summary>
        [ConfigurationProperty("workSpaceNameSniffer", DefaultValue = "", IsRequired = false, IsKey = false)]
        string WorkSpaceNameSniffer { get; set; }

        /// <summary>
        /// Get the DBConnectionString for the DB where <paramref name="entityType"/> is persistent in
        /// </summary>
        /// <returns></returns>
        string DBConnectionString(System.Type entityType);
    }
}