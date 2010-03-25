using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Envers.Net.Model
{
    [Class(Name="Person")]
    public class Person : DomainObject
    {
        private String fullName;
        [Property]
        public virtual String firstName{ get; set; }
        [Property]
        public virtual String lastName { get; set; }
        [ManyToOne]
        public virtual Address address { get; set; }
    }
}