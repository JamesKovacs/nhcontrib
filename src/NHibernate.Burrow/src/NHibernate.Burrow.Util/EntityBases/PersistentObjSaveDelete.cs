namespace NHibernate.Burrow.Util.EntityBases {
    ///<summary>
    /// Persistence Object that has the public Save and Delete method
    ///</summary>
    /// <remarks>
    /// This can be a super class for object that is allowed to be saved or deleted externally. 
    /// If your class want to control the Save and Delete, please use <see cref="PersistentObj"/>
    /// </remarks>
    public abstract class PersistentObjSaveDelete : PersistentObj, IPersistentObjSaveDelete {
        #region IPersistentObjSaveDelete Members

        /// <summary>
        /// Persists the object into database
        /// </summary>
        public virtual void Save() {
            dao.Save();
        }

        /// <summary>
        /// Tells if this object is transient (not saved to database yet)
        /// </summary>
        public bool IsTransient {
            get { return dao.IsTransient; }
        }

        /// <summary>
        /// Remove the object from the database 
        /// </summary>
        public virtual void Delete() {
            dao.Delete();
        }

        #endregion
    }
}