using System.Collections;
using System.Configuration;

namespace NHibernate.Burrow.Configuration {
    /// <summary>
    /// Configuration Section for a Persistant Unit
    /// </summary>
    public class PUSection : ConfigurationSection {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("name",
            DefaultValue = "PersistantUnit1",
            IsRequired = true,
            IsKey = true)]
        [StringValidator(InvalidCharacters =
                         " ~!@#$%^&*()[]{}/;'\"|\\",
            MinLength = 1, MaxLength = 60)]
        public string Name {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("enableAuditLog",
            DefaultValue = "false",
            IsRequired = false,
            IsKey = false)]
        public bool EnableAuditLog {
            get { return (bool) this["enableAuditLog"]; }
            set { this["enableAuditLog"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("domainLayerAssemblies",
            IsDefaultCollection = false)]
        public DomainLayerAssemblyElementCollection DomainLayerAssemblies {
            get { return (DomainLayerAssemblyElementCollection) base["domainLayerAssemblies"]; }
        }

        /// <summary>
        /// setting related to the ORM frameworks such as NHibernate
        /// </summary>
        [ConfigurationProperty("ORMFrameworkSettings",
            IsDefaultCollection = false)]
        public KeyValueConfigurationCollection ORMFrameworkSettings {
            get { return (KeyValueConfigurationCollection) base["ORMFrameworkSettings"]; }
        }

        /// <summary>
        /// The ORMFrameworkSettings oganzied into a dictionary
        /// </summary>
        public IDictionary ORMFrameworkSettingsDict {
            get { return CopyToDict(ORMFrameworkSettings); }
        }

        private IDictionary CopyToDict(KeyValueConfigurationCollection settings) {
            IDictionary dict = new Hashtable(settings.Count);
            foreach (KeyValueConfigurationElement setting in settings) dict.Add(setting.Key, setting.Value);
            return dict;
        }
    }
}