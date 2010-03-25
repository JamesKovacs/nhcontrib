using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Envers.Net.Model
{
    [Serializable]
    public class DomainObject
    {
        [Id(Name = "id", Type = "long")]
        [Generator(1, Class = "native")]
        public virtual long id { get; set; }
        [Version]
        public virtual int version { get; set; }
    }
}
