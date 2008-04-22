namespace NHibernate.Burrow
{
    public interface IPersistenceUnitCfg
    {
        string Name { get; set; }

        string NHConfigFile { get; set; }

        bool ManualTransactionManagement { get; set; }

        /// <summary>
        /// designates the implementation of IInterceptorFactory with which Burrow will create managed Session 
        /// </summary>
        string InterceptorFactory { get; set; }
    }
}