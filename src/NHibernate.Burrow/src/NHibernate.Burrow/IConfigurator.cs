using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow
{
    /// <summary>
    /// implement this to modify the configuration before they are used.
    /// </summary>
    public interface IConfigurator
    {
        void Config(IBurrowConfig val);
        void Config(IPersistenceUnitCfg puCfg, Cfg.Configuration nhCfg);
    }
}
