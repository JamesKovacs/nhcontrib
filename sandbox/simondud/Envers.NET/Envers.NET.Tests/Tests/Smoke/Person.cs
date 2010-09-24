using System;
using NHibernate.Mapping.Attributes;
using NHibernate.Envers;

namespace NHibernate.Envers.Tests.Smoke
{
    [Class(Name="Person")]
    [Audited]
    public class Person : DomainObject
    {
        private String fullName;
        [Property]
        public virtual String firstName{ get; set; }
        [Property]
        public virtual String lastName { get; set; }
        [ManyToOne]
        public virtual Address address { get; set; }
        [ManyToOne]
        [Audited(TargetAuditMode = RelationTargetAuditMode.NOT_AUDITED)]
        public virtual ContBancar cont { get; set; }
    }
}