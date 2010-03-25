using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers.Configuration.Metadata
{
    /**
     * Holds information necessary to create an audit table: its name, schema and catalog, as well as the audit
     * entity name.
     * @author Adam Warski (adam at warski dot org)
     */
    public class AuditTableData {
        private readonly String auditEntityName;
        private readonly String auditTableName;
        private readonly String schema;
        private readonly String catalog;

        public AuditTableData(String auditEntityName, String auditTableName, String schema, String catalog) {
            this.auditEntityName = auditEntityName;
            this.auditTableName = auditTableName;
            this.schema = schema;
            this.catalog = catalog;
        }

        public String getAuditEntityName() {
            return auditEntityName;
        }

        public String getAuditTableName() {
            return auditTableName;
        }

        public String getSchema() {
            return schema;
        }

        public String getCatalog() {
            return catalog;
        }
    }
}
