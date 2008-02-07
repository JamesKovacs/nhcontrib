using System;
using System.Collections.Generic;
using NHibernate.Expression;
namespace NHibernate.Burrow.NHDomain.BizTransactionEntity {
    ///<summary>
    /// a base DAO for BizTransactionEntity
    ///</summary>
    ///<typeparam name="ReturnT"></typeparam>
    ///<typeparam name="NHT"></typeparam>
    /// <remarks>
    /// This dao will help manage the life circle of BizTransactionEntity
    /// that is delete the timeout one and update the LastActivityInTransaction automatically and etc.
    /// </remarks>
    public abstract class BizTransactionEntityDAOBase<ReturnT, NHT> : AdvGenericDAO<ReturnT, NHT>
        where NHT : IBizTransactionEntity {
        private static readonly object lockObj = new object();
        private static readonly TimeSpan timeout = new TimeSpan(24, 0, 0);
        private static DateTime lastClearOutDatedTime = DateTime.Now;

        protected virtual string FinishedTransactionProperyName {
            get { return "FinishedTransaction"; }
        }

        protected virtual string LastActivityInTransactionProperyName {
            get { return "LastActivityInTransaction"; }
        }

        protected override void OnSave(ReturnT t) {
            ClearTimedOutEntities();
            base.OnSave(t);
        }

        protected override void OnLoad(ReturnT loaded) {
            IBizTransactionEntity entity = (IBizTransactionEntity) loaded;
            if (!entity.FinishedTransaction)
                entity.LastActivityInTransaction = DateTime.Now;
            base.OnLoad(loaded);
        }

        private void ClearTimedOutEntities() {
            if (DateTime.Now > lastClearOutDatedTime + timeout) {
                lastClearOutDatedTime = DateTime.Now;
                lock (lockObj) {
                    foreach (NHT t in FindTimedOutOnes())
                        t.Delete();
                }
            }
        }

        private IList<NHT> FindTimedOutOnes() {
            return GetCriteria().Add(NHibernate.Expression.Expression.Eq(FinishedTransactionProperyName, false))
                .Add(NHibernate.Expression.Expression.Le(LastActivityInTransactionProperyName, DateTime.Now - timeout))
                .List<NHT>();
        }
        }
}