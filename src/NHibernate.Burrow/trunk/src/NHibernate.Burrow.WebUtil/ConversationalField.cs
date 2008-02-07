using System;

namespace NHibernate.Burrow.WebUtil {
    [AttributeUsage(AttributeTargets.Field)]
    public class ConversationalField : StatefulField {
        public ConversationalField() : base(typeof (ConversationalDataVSFInterceptor).AssemblyQualifiedName) {}
    }
}