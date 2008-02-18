using System.Configuration;

namespace NHibernate.Burrow.Configuration {
    /// <summary>
    /// Root Section for NHibernate.Burrow Configuration
    /// </summary>
    public class NHibernateBurrowCfgSection : ConfigurationSection {
        /// <summary>
        /// 
        /// </summary>
        public const string SectionName = "NHibernate.Burrow";

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Declare a collection element represented 
        /// in the configuration file by the sub-section
        /// <![CDATA[
        /// <persistantUnits> <add .../> </persistantUnits> 
        /// Note: the "IsDefaultCollection = false" 
        /// instructs the .NET Framework to build a nested 
        /// section like <persistantUnits> ...</persistantUnits>.
        /// ]]>
        /// </remarks>
        [ConfigurationProperty("persistantUnits",
            IsDefaultCollection = false)]
        public PersistenceUnitElementCollection PersistenceUnits {
            get {
                PersistenceUnitElementCollection persistantUnits =
                    (PersistenceUnitElementCollection) base["persistantUnits"];
                return persistantUnits;
            }
        }

        ///<summary>
        /// The converstaion timout minutes
        ///</summary>
        [ConfigurationProperty("conversationTimeOut",
            DefaultValue = "30",
            IsRequired = false,
            IsKey = false)]
        public int ConversationTimeOut {
            get { return (int) this["conversationTimeOut"]; }
            set { this["conversationTimeOut"] = value; }
        }

        ///<summary>
        /// The conversation clean up frequency,
        ///  for how many times of conversation Timeout,
        ///  the conversation pool clean up its timeoutted converstaions.
        ///  must be greater than 1
        ///</summary>
        [ConfigurationProperty("conversationCleanupFrequency",
            DefaultValue = "4",
            IsRequired = false,
            IsKey = false)]
        public int ConversationCleanupFrequency {
            get { return (int) this["conversationCleanupFrequency"]; }
            set { this["conversationCleanupFrequency"] = value; }
        }

        /// <summary>
        /// Get the instance from the current application's config file
        /// </summary>
        /// <returns></returns>
        public static NHibernateBurrowCfgSection GetInstance() {
            NHibernateBurrowCfgSection section = ConfigurationManager.GetSection(SectionName) as NHibernateBurrowCfgSection;
            if (section == null)
                throw new DomainTemplateException("Section \"" + SectionName + "\" is not found");
            return section;
        }
    }
}