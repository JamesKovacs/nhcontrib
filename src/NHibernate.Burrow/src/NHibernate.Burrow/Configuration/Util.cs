using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Configuration
{
    internal class Util
    {
        public void CheckCanChangeCfg()
        {
            BurrowFramework f = new BurrowFramework();
            if (f.BurrowEnvironment.IsRunning)
            {
                throw new ChangeConfigWhenRunningException(
                    "Configuration Setting can only be changed on the fly when the environment is shut down");
            }
        }
    }
}