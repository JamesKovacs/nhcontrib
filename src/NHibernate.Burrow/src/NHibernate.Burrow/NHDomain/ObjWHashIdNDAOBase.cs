using System;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// A base class for object that has HashId and DAO
    /// </summary>
    public abstract class ObjWHashIdNDAOBase : ObjectWHashIdBase, IPersistantObjWithDAO {
        private IObjectDAOHelper dao;

        #region IPersistantObjWithDAO Members

        /// <summary>
        /// A DAO helper that helps with the Database Access related function for the entity
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