using System;
using System.Web;
using NHibernate.Burrow.Util.DomainSession;
using NHibernate.Burrow.Util.EntityBases;

namespace NHibernate.Burrow.Util.AuditLog {
    /// <summary>
    /// A record of Audit log
    /// </summary>
    public class AuditLogRecord {
        #region fields and properties

        private string action;
        private DateTime created = DateTime.Now;
        private IWithId entity;
        private string entityId;
        private string entityType;
        private int id;
        private string newValue;
        private string oldValue;
        private string propertyName;
        private string propertyType;
        private string userId;
        private string userName;

        /// <summary>
        /// The entity of this log, it's not persistant
        /// </summary>
        public IWithId Entity {
            get { return entity; }
            set { entity = value; }
        }

        /// <summary>
        /// The Id of the Entity stored in the string format
        /// </summary>
        public string EntityId {
            get { return entityId; }
            set { entityId = value; }
        }

        /// <summary>
        /// Gets and Sets when the audit log 
        /// </summary>
        public DateTime Created {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// the id of the user that  performed the database change 
        /// </summary>
        public string UserId {
            get { return userId; }
            set { userId = value; }
        }

        /// <summary>
        /// the username of the user that performed the database change
        /// </summary>
        public string UserName {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// the type of the property changed.
        /// </summary>
        public string PropertyType {
            get { return propertyType; }
            set { propertyType = value; }
        }

        /// <summary>
        /// the new value of the property after change
        /// </summary>
        public string NewValue {
            get { return newValue; }
            set { newValue = value; }
        }

        /// <summary>
        /// the original value prior to change
        /// </summary>
        public string OldValue {
            get { return oldValue; }
            set { oldValue = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyName {
            get { return propertyName; }
            set { propertyName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string EntityType {
            get { return entityType; }
            set { entityType = value; }
        }

        /// <summary>
        /// The name of the action user did to this record
        /// </summary>
        /// <see cref="Actions"/>
        public string Action {
            get { return action; }
            set { action = value; }
        }

        /// <summary>
        /// The id of the record
        /// </summary>
        public int Id {
            get { return id; }
            private set { id = value; }
        }

        #endregion

        private AuditLogRecord() {}

        internal static AuditLogRecord Create() {
            AuditLogRecord retVal = new AuditLogRecord();

            if (DSConfig.IsUsingDomainLayer
                && DomainSessionContainer.DomainSession != null
                && DomainSessionContainer.DomainSession is IHasUserId)
                retVal.UserId = ((IHasUserId) DomainSessionContainer.DomainSession).UserId + string.Empty;
            if (HttpContext.Current != null
                && HttpContext.Current.User != null
                && HttpContext.Current.User.Identity != null)
                retVal.UserName = HttpContext.Current.User.Identity.Name;

            return retVal;
        }

        /// <summary>
        /// Ensure the EnitityId is set to the Entity's Id
        /// </summary>
        /// <remarks>
        /// This method is created for the AuditLogInterceptor to call after the entity obtain its Id. 
        /// </remarks>
        public void EnsureEntityId() {
            if (Entity != null)
                EntityId = Entity.Id.ToString();
        }
    }
}