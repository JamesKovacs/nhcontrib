using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers
{
    /**
     * When applied to a field, indicates that this field should not be audited.
     * @author Simon Duduica, port of Envers omonyme class by Sebastian Komander
     */
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    class NotAuditedAttribute : Attribute
    {
    }
}
