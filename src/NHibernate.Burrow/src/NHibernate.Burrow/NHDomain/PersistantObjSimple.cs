/*
 * Created by: Kai Wang
 * Created: Tuesday, February 06, 2007
 */

using System;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// Can be a base for the children entity
    /// </summary>
    /// <remarks>
    /// for some entity that only exists as a composite children of other entity, no business key or over session equality is needed.
    /// </remarks>
    public abstract class PersistantObjSimple : IWithId {
        private int id;

        public int Id {
            get { return id; }
            private set { id = value; }
        }
        /// <summary>
        /// a helper for inheritance to perform DAO functions
        /// </summary>
        protected IObjectDAOHelper dao;

        /// <summary>
        /// 
        /// </summary>
        public PersistantObjSimple()
        {
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