using System.Configuration;

namespace NHibernate.Burrow {
    public interface IPersistenceUnitCfg {
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
        string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("nh-config-file",
            IsRequired = true,
            IsKey = false)]
        [StringValidator(InvalidCharacters =
                         " ~!@#$%^&*()[]{};'\"|",
            MaxLength=160)]
        string NHConfigFile { get; set; }
    }
}