using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;
using NHibernate.Envers;

namespace NHibernate.Envers.Tests.Smoke
{
    [Audited]
    [Class(Name="Address")]
    public class Address : DomainObject
    {
        [Property]
        public virtual String street { get; set; }
        [Property]
        public virtual String number { get; set; }
    }
}
