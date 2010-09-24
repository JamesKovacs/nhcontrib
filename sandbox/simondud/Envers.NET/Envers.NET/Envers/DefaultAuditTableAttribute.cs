using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers
{
    class DefaultAuditTableAttribute : AuditTableAttribute
    {
        public System.Type AttributeType()
        {
            return typeof(DefaultAuditTableAttribute);
        }
    }
}
