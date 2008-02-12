namespace NHibernate.Burrow {
    /// <summary>
    /// A factory for creating Interceptor
    /// </summary>
    public interface IInterceptorFactory {
        /// <summary>
        /// Create the Interceptor
        /// </summary>
        /// <returns></returns>
        IInterceptor Create();
    }
}