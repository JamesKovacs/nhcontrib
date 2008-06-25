using System.Configuration;

namespace NHibernate.Burrow
{
    public interface IPersistenceUnitCfg
    {
        string Name { get; set; }

        string NHConfigFile { get; set; }
 
        /// <summary>
        /// designates the implementation of IInterceptorFactory with which Burrow will create managed Session 
        /// </summary>
        string InterceptorFactory { get; set; }

    	///<summary>
    	/// whether Burrow should automatically update the schema for this persistant unit, default is false  
    	///</summary>
    	[ConfigurationProperty("autoUpdateSchema", DefaultValue = false, IsRequired = false, IsKey = false)]
    	bool AutoUpdateSchema
    	{
    		get;
    		set;
    	}
    }
}