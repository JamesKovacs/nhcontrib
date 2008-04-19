using System;

namespace NHibernate.Burrow.WebUtil.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ConversationalField : StatefulField
    {
        public ConversationalField() : base(typeof (ConversationalDataVSFInterceptor).AssemblyQualifiedName) {}
    }
}