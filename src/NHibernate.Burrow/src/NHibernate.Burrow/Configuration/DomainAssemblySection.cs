using System.Configuration;

namespace NHibernate.Burrow.Configuration {
    /// <summary>
    /// 
    /// </summary>
    public class DomainAssemblySection : ConfigurationSection {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("name",
            DefaultValue = "MindHarbor.ProjectDomainLayer",
            IsRequired = true,
            IsKey = true)]
        public string Name {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("skipSorting",
            DefaultValue = "false",
            IsRequired = false,
            IsKey = false)]
        public bool SkipSorting {
            get { return (bool) this["skipSorting"]; }
            set { this["skipSorting"] = value; }
        }

        /// <summary>
        /// setting related to the ORM frameworks such as NHibernate
        /// </summary>
        [ConfigurationProperty("domainAssemblySettings",
            IsDefaultCollection = false)]
        public KeyValueConfigurationCollection DomainAssemblySettings {
            get { return (KeyValueConfigurationCollection) base["domainAssemblySettings"]; }
        }
    }
}