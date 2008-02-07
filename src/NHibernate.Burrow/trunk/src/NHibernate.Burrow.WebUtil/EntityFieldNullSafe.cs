using System;

namespace NHibernate.Burrow.WebUtil {
    [AttributeUsage(AttributeTargets.Field)]
    public class EntityFieldNullSafe : StatefulField {
        public EntityFieldNullSafe()
            : base(typeof (GetEntityVSFInterceptor).AssemblyQualifiedName) {}
    }
}