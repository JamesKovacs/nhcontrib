using System;
using NHibernate.Burrow.WebUtil.Impl;

namespace NHibernate.Burrow.WebUtil.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EntityField : StatefulField
    {
        public EntityField() : base(typeof (LoadEntityVSFInterceptor).AssemblyQualifiedName) {}
    }
}