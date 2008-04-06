using System.Configuration;

namespace NHibernate.Burrow {
    public interface IPersistenceUnitCfg {
       
        string Name { get; set; }

      
        string NHConfigFile { get; set; }

        bool ManualTransactionManagement
        {
            get;
            set;
        }
    }
}