using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuditTableAttribute : Attribute
    {
        public String value;

        /**
         * @return The schema of the table. Defaults to the schema of the annotated entity.
         */
        public String schema = "";

        /**
         * @return The catalog of the table. Defaults to the catalog of the annotated entity.
         */
        public String catalog = "";
    }
}
