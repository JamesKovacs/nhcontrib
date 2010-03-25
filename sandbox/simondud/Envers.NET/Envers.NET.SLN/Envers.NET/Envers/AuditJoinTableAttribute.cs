using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Method)]
    public class AuditJoinTableAttribute : Attribute
    {
    /**
     * @return Name of the join table. Defaults to a concatenation of the names of the primary table of the entity
     * owning the association and of the primary table of the entity referenced by the association.
     */
    public String name = "";

    /**
     * @return The schema of the join table. Defaults to the schema of the entity owning the association.
     */
    public String schema = "";

    /**
     * @return The catalog of the join table. Defaults to the catalog of the entity owning the association.
     */
    public String catalog = "";

    /**
     * @return The foreign key columns of the join table which reference the primary table of the entity that does not
     * own the association (i.e. the inverse side of the association).
     */
    public JoinColumnAttribute[] inverseJoinColumns = {};
    }
}
