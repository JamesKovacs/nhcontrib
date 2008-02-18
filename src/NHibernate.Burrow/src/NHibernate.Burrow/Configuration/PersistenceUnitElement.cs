using System.Collections;
using System.Configuration;

namespace NHibernate.Burrow.Configuration {
    /// <summary>
    /// Configuration Section for a Persistence Unit
    /// </summary>
    public class PersistenceUnitElement : ConfigurationElement{
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("name",
            DefaultValue = "PersistenceUnit1",
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
        [ConfigurationProperty("nh-config-file",
            IsRequired = true,
            IsKey = false)]
        [StringValidator(InvalidCharacters =
                         " ~!@#$%^&*()[]{}/;'\"|\\",
              MaxLength=160)]
        public string NHConfigFile {
            get { return (string)this["nh-config-file"]; }
            set { this["nh-config-file"] = value; }
        }
 
    }
}