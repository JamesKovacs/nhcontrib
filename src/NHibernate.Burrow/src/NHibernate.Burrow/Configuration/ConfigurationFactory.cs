using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Configuration
{
    internal class ConfigurationFactory
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurator">optional</param>
        /// <returns></returns>
        public IBurrowConfig Create(IConfigurator configurator)
        {
            IBurrowConfig retVal = GetConfigurationInstance();
            if(configurator != null)
                retVal.Configurator = configurator;
            
            if (retVal.Configurator != null)
                retVal.Configurator.Config(retVal);
            return retVal;
        }

        protected virtual IBurrowConfig GetConfigurationInstance()
        {
            return NHibernateBurrowCfgSection.CreateInstance();
        }
    }
}
