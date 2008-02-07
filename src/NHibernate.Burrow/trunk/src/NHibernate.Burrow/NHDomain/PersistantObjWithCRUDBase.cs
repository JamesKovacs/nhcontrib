using System;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// OBSOLETE - it's too dangerous to allow client to use Id as business key
    /// This class is for the convenience for the extremely simple projects. 
    /// This class offers CRUD methods and also override Bizkey with Id, So if not overrriden you will experience weird problem when playing with new object 
    /// If you are not sure, use PersistantObjWithDAOBase whenever possible. 
    /// </summary>
    [Obsolete]
    public abstract class PersistantObjWithCRUDBase : PersistantObjWithDAOBase, IPersistantObjWithCRUD {
        #region IPersistantObjWithCRUD Members

        /// <summary>
        /// 
        /// </summary>
        public virtual void Delete() {
            DAO.Delete();
        }

        /// <summary>
        ///  Refresh the object from database
        /// </summary>
        public void Refresh() {
            DAO.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveOrUpdate() {
            if (DAO.IsTransient)
                DAO.SaveOrUpdate();
            else
                Update();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update() {
            DAO.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save() {
            DAO.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReAttach() {
            DAO.ReAttach();
        }

        /// <summary>
        /// the database Id businessKey
        /// </summary>
        /// <remarks>
        /// The businesskey is overriden to use Database Id. This is mainly for the sake of compatibility with some old projects. 
        /// override it to use your own businesskey whenever possible
        /// </remarks>
        public override IComparable BusinessKey {
            get {
                if (Id == 0)
                    return GetHashCode();
                return Id;
            }
        }

        #endregion
    }
}