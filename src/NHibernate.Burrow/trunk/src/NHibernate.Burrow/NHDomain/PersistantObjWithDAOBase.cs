using System;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// OBSOLETE
    /// A simple implementation of the IPersistantObjWithDAO
    /// </summary>
    [Obsolete]
    public abstract class PersistantObjWithDAOBase : ObjWithIdNBizKeyBase, IPersistantObjWithDAO {
        private IObjectDAOHelper dao;

        #region IPersistantObjWithDAO Members

        /// <summary>
        /// the helper<see cref="IObjectDAOHelper"/> for doing the DAO related work
        /// </summary>
        public IObjectDAOHelper DAO {
            get {
                if (dao == null) {
                    dao = new ObjectDAOHelper(this);
                    dao.PreDeleted += new EventHandler(OnPreDeleted);
                }
                return dao;
            }
        }

        #endregion

        /// <summary>
        /// will be called when this object is going to be deleted;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnPreDeleted(object sender, EventArgs e) {}
    }
}