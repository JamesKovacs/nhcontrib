/*
 * Created by: Kai Wang
 * Created: Tuesday, February 06, 2007
 */

using System;
using NHibernate.Burrow.Util.DAOBases;

namespace NHibernate.Burrow.Util.EntityBases {
    /// <summary>
    /// Targeted to be the standard PersistantObj
    /// </summary>
    public abstract class PersistantObj : ObjWithIdNBizKeyBase {
        /// <summary>
        /// a helper for inheritance to perform DAO functions
        /// </summary>
        protected IObjectDAOHelper dao;

        /// <summary>
        /// 
        /// </summary>
        public PersistantObj() {
            dao = new ObjectDAOHelper(this);
            dao.PreDeleted += new EventHandler(OnPreDeleted);
        }

        /// <summary>
        /// will be called when this object is going to be deleted;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPreDeleted(object sender, EventArgs e) {}
    }
}