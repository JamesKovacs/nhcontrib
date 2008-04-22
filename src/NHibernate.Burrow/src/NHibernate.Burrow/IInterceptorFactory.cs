namespace NHibernate.Burrow
{
    /// <summary>
    /// implement this and set the type in the <see cref="IPersistenceUnitCfg.InterceptorFactory"/> so that Burrow will use <see cref="Create"/> when creating the managed Session
    /// </summary>
    public interface IInterceptorFactory
    {
        /// <summary>
        /// Creates the IInterceptor with which Burrow will create managed Session 
        /// </summary>
        IInterceptor Create(Cfg.Configuration cfg);
    }
}