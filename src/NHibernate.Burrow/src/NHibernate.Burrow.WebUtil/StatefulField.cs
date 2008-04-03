using System;
using NHibernate.Burrow.Util;

namespace NHibernate.Burrow.WebUtil {
    /// <summary>
    /// Mark the field to span over multiple requests/responses for a control
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StatefulField : Attribute {
        private readonly string interceptorName;

        public StatefulField() {}

        public StatefulField(string interceptor) {
            interceptorName = interceptor;
        }

        public IStatefulFieldInterceptor Interceptor {
            get {
                if (string.IsNullOrEmpty(interceptorName))
                    return null;
                return InstanceLoader.Load<IStatefulFieldInterceptor>(interceptorName);
            }
        }
    }
}