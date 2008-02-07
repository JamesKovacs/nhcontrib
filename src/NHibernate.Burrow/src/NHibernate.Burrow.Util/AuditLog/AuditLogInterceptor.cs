using System.Collections;
using Iesi.Collections.Generic;
using NHibernate.Burrow.NHDomain;
using NHibernate.Burrow.Util.EntityBases;
using NHibernate.Type;

namespace NHibernate.Burrow.Util.AuditLog {
    internal class AuditLogInterceptor : IInterceptor {
        private const int valueTruncateLength = 1000;
        private ISet<AuditLogRecord> records = new HashedSet<AuditLogRecord>();

        #region IInterceptor Members

        public bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState,
                                 string[] propertyNames, IType[] types) {
            for (int i = 0; i < currentState.Length; i++)
                AddLogRecord(entity,
                             id,
                             currentState[i],
                             previousState != null ? previousState[i] : null,
                             propertyNames[i],
                             types[i],
                             Actions.UPDATE);
            return false;
        }

        public bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types) {
            AddLogRecord(entity, id, null, null, null, null, Actions.INSERT);
            for (int i = 0; i < state.Length; i++)
                AddLogRecord(entity,
                             id,
                             state[i],
                             string.Empty,
                             propertyNames[i],
                             types[i],
                             Actions.INSERT_UPDATE
                    );

            return false;
        }

        public void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types) {
            AddLogRecord(entity, id, null, null, null, null, Actions.DELETE);
        }

        public void PostFlush(ICollection entities) {
            ISession sess = SessionManager.Instance.GetUnManagedSession();
            try {
                ITransaction t = sess.BeginTransaction();
                foreach (AuditLogRecord record in records) {
                    record.EnsureEntityId();
                    sess.Save(record);
                }
                records.Clear();
                t.Commit();
            }
            finally {
                sess.Close();
            }
        }

        ///<summary>
        ///
        ///            Called just before an object is initialized
        ///            
        ///</summary>
        ///
        ///<param name="entity"></param>
        ///<param name="id"></param>
        ///<param name="propertyNames"></param>
        ///<param name="state"></param>
        ///<param name="types"></param>
        ///<remarks>
        ///
        ///            The interceptor may change the 
        ///<c>state</c>, which will be propagated to the persistent
        ///            object. Note that when this method is called, 
        ///<c>entity</c> will be an empty
        ///            uninitialized instance of the class.
        ///</remarks>
        ///
        ///<returns>
        ///
        ///<c>true</c> if the user modified the 
        ///<c>state</c> in any way
        ///</returns>
        ///
        public bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types) {
            return false;
        }

        public void PreFlush(ICollection entities) {
            return;
        }

        public object IsUnsaved(object entity) {
            return null;
        }

        public int[] FindDirty(object entity, object id, object[] currentState, object[] previousState,
                               string[] propertyNames, IType[] types) {
            return null;
        }

        public object Instantiate(System.Type type, object id) {
            return null;
        }

        ///<summary>
        ///            Called when a NHibernate transaction is begun via the NHibernate <see cref="T:NHibernate.ITransaction" />
        ///            API. Will not be called if transactions are being controlled via some other mechanism.
        ///</summary>
        public void AfterTransactionBegin(ITransaction tx) {}

        ///<summary>
        ///
        ///            Called before a transaction is committed (but not before rollback).
        ///            
        ///</summary>
        ///
        public void BeforeTransactionCompletion(ITransaction tx) {}

        ///<summary>
        ///            Called after a transaction is committed or rolled back.
        ///</summary>
        public void AfterTransactionCompletion(ITransaction tx) {}

        public void SetSession(ISession session) {
            //don't know how to implement this method.
        }

        #endregion

        private void AddLogRecord(object entity,
                                  object id,
                                  object currentState,
                                  object previousState,
                                  string propertyName,
                                  IType type,
                                  string action) {
            if (StatesEquals(currentState, previousState) && action == Actions.UPDATE)
                return;
            AuditLogRecord alg = AuditLogRecord.Create();
            alg.Action = action;
            alg.EntityType = entity == null
                                 ? "Unkown Entity"
                                 : alg.EntityType = entity.GetType().Name;
            alg.Entity = entity as IWithId;
            alg.EntityId = id == null ? "Unknown Id" : id.ToString();
            alg.NewValue = ToTruncatedString(currentState);
            alg.OldValue = ToTruncatedString(previousState);
            alg.PropertyName = propertyName;
            if (type != null)
                alg.PropertyType = type.Name;
            records.Add(alg);
        }

        private bool StatesEquals(object o, object o1) {
            if (o == o1)
                return true;
            else if (o != null)
                return o.Equals(o1);
            else
                return o1 == null;
        }

        private string ToTruncatedString(object o) {
            if (o == null) return string.Empty;
            string retVal = o.ToString();
            if (retVal.Length > valueTruncateLength)
                retVal = retVal.Substring(0, valueTruncateLength);
            return retVal;
        }
    }
}