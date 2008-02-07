using System;
using NHibernate.Burrow.NHDomain;
using NHibernate.Burrow.Util.EntityBases;

namespace NHibernate.Burrow.Util.DAOBases {
    internal class ObjectDAOHelper : IObjectDAOHelper {
        private GenericDAO<object> gdao;
        private bool isDeleted = false;

        private IWithId obj;

        public ObjectDAOHelper(IWithId obj) {
            this.obj = obj;
            gdao = new GenericDAO<object>(obj.GetType().Assembly);
        }

        #region IObjectDAOHelper Members

        public event EventHandler PreDeleted;

        public bool IsDeleted {
            get { return isDeleted; }
        }

        public bool IsTransient {
            get { return obj.Id <= 0; }
        }

        public virtual void Update() {
            gdao.Update(obj);
        }

        /// <summary>
        /// Won't throw exception if the obj is either already deleted or still transient
        /// </summary>
        public void Delete() {
            if (IsDeleted)
                return;
            isDeleted = true;
            //need to make sure that even the object is transient the preDelete still gets fired
            if (PreDeleted != null)
                PreDeleted(this, new EventArgs());

            if (!IsTransient)
                gdao.Delete(obj);
        }

        public void SaveOrUpdate() {
            if (isDeleted) throw new DomainException("Can not saveorUpdate once deleted");
            gdao.SaveOrUpdate(obj);
        }

        public void Save() {
            if (isDeleted) throw new DomainException("Can not saveorUpdate once deleted");
            gdao.Save(obj);
        }

        public void ReAttach() {
            if (isDeleted) throw new DomainException("Can not saveorUpdate once deleted");
            gdao.ReAttach(obj);
        }

        public void Refresh() {
            if (!IsTransient && !IsDeleted)
                gdao.Refresh(obj);
        }

        #endregion
    }
}