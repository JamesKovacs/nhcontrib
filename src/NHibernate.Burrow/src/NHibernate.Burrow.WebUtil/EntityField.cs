using System;

namespace NHibernate.Burrow.WebUtil {
    [AttributeUsage(AttributeTargets.Field)]
    public class EntityField : StatefulField {
        public EntityField() : base(
            typeof (LoadEntityVSFInterceptor).AssemblyQualifiedName) {}
    }
}