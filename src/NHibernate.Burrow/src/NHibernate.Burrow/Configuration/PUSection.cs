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
        ///  //Temporarily removed auditLog before we decided whether it should stay in Burrow
        //[ConfigurationProperty("enableAuditLog",
        //    DefaultValue = "false",
        //    IsRequired = false,
        //    IsKey = false)]
        //public bool EnableAuditLog {
        //    get { return (bool) this["enableAuditLog"]; }
        //    set { this["enableAuditLog"] = value; }
        //}
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("domainLayerAssemblies",
            IsDefaultCollection = false)]
        public DomainAssemblySectionCollection DomainAssemblies {
            get { return (DomainAssemblySectionCollection) base["domainLayerAssemblies"]; }
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
            get { return Util.CopyToDict(ORMFrameworkSettings); }
        }
    }
}