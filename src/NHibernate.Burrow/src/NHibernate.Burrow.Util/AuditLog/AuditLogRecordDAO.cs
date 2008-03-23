using System.Collections.Generic;
using NHibernate.Burrow.Util.DAOBases;
using NHibernate.Criterion;

namespace NHibernate.Burrow.Util.AuditLog {
    /// <summary>
    /// 
    /// </summary>
    public class AuditLogRecordDAO : GenericDAO<AuditLogRecord> {
        private static readonly AuditLogRecordDAO instance = new AuditLogRecordDAO();

        /// <summary>
        /// 
        /// </summary>
        public static AuditLogRecordDAO Instance {
            get { return instance; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public IList<AuditLogRecord> Find(System.Type entityType, object entityId) {
            return GetCriteria().Add(Expression.Eq("EntityId", entityId.ToString()))
                .Add(Expression.Eq("EntityType", entityType.Name.ToString()))
                .AddOrder(new Order("Created", false))
                .List<AuditLogRecord>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public IList<AuditLogRecord> Find(System.Type entityType, object entityId, string action) {
            return GetCriteria().Add(Expression.Eq("EntityId", entityId.ToString()))
                .Add(Expression.Eq("EntityType", entityType.Name.ToString()))
                .Add(Expression.Eq("Action", action))
                .List<AuditLogRecord>();
        }
    }
}