using System;
using System.Configuration;

namespace NHibernate.Burrow.Configuration {
    /// <summary>
    /// 
    /// </summary>
    public class DomainLayerAssemblyElement : ConfigurationElement {
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
        /// 
        /// </summary>
        [Obsolete]
        [ConfigurationProperty("domianLayerFactoryName", IsRequired = false)]
        [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
        public string DomianLayerFactoryName {
            get { return (string) this["domianLayerFactoryName"]; }
            set { this["domianLayerFactoryName"] = value; }
        }  
        
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("domianSessionFactoryName", IsRequired = false)]
        [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\")]
        public string DomianSessionFactoryName {
            get { return (string)this["domianSessionFactoryName"]; }
            set { this["domianSessionFactoryName"] = value; }
        }
    }
}