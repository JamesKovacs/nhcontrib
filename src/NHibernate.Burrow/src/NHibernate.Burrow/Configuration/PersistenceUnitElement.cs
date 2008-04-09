using System.Collections.Generic;
using System.Configuration;

namespace NHibernate.Burrow.Configuration {
    /// <summary>
    /// Configuration Section for a Persistence Unit
    /// </summary>
    public class PersistenceUnitElement : ConfigurationElement, IPersistenceUnitCfg {

        //Todo: eliminate the duplicate code in this class and NHiberanteBurrowCfgSection
        private readonly IDictionary<string, object> savedSettings = new Dictionary<string, object>();
        private void Set(string key, object value)
        {
            new Util().CheckCanChangeCfg(); 
            savedSettings[key] = value;
        }

        private object Get(string key)
        {
            if (savedSettings.ContainsKey(key))
                return savedSettings[key];
            else
                return this[key];
        }

        
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
            get { return (string) Get("name"); }
            set { Set("name", value); }
        } 
        
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("nh-config-file",
            IsRequired = true,
            IsKey = false)]
        [StringValidator(InvalidCharacters =
                         " !@#$%^&*()[]{};'\"|",
              MaxLength=160)]
        public string NHConfigFile {
            get { return (string)Get("nh-config-file"); }
            set { Set("nh-config-file", value); }
        }

        ///<summary>
        /// wheather the transaction under this persistence Unit is manually managed by client    
        ///</summary>
        [ConfigurationProperty("manualTransactionManagement", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool ManualTransactionManagement
        {
            get { return (bool)Get("manualTransactionManagement"); }
            set { Set("manualTransactionManagement", value); }
        }

 
    }
}
