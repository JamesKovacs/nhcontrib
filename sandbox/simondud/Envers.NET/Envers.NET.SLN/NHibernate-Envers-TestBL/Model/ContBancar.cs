using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;
using NHibernate.Envers;

namespace Envers.Net.Model
{
    [Class(Name="ContBancar")]
    public class ContBancar : DomainObject
    {
        [Property]
        public virtual String NumeBanca { get; set; }
        [Property]
        public virtual String IBAN{ get; set; }
        [ManyToOne]
        public virtual Address Adresa { get; set; }
    }
}
