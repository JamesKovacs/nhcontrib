using NHibernate;

namespace NHibernate.Burrow.NHDomain {
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