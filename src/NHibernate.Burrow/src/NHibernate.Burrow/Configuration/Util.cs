using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using NHibernate.Burrow.Impl;

namespace NHibernate.Burrow.Configuration
{
    internal class Util
    {
        public void CheckCanChangeCfg() {
            Facade f = new Facade();
            if( f.BurrowEnvironment.IsRunning )
                throw new Exceptions.ChangeConfigWhenRunningException("Configuration Setting can only be changed on the fly when the environment is shut down");
        }
    }
}
