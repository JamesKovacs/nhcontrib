using NHibernate;

namespace NHibernate.Burrow.NHDomain.AuditLog {
    /// <summary>
    /// Factory of IInterceptor
    /// </summary>
    public class AuditLogInterceptorFactory : IInterceptorFactory {
        private static readonly AuditLogInterceptorFactory instance = new AuditLogInterceptorFactory();

        private AuditLogInterceptorFactory() {}

        /// <summary>
        /// Gets the singleton instance 
        /// </summary>
        public static AuditLogInterceptorFactory Instance {
            get { return instance; }
        }

        #region IInterceptorFactory Members

        /// <summary>
        /// Create the Interceptor
        /// </summary>
        /// <returns></returns>
        public IInterceptor Create() {
            return new AuditLogInterceptor();
        }

        #endregion
    }
}