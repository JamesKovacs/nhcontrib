using System;
using NHibernate.Burrow.AppBlock.DAOBases;

namespace NHibernate.Burrow.AppBlock.EntityBases
{
    public abstract class DeletableEntity : IDeletable
    {
        private bool deleted;

        public DeletableEntity()
        {
            long result = (long) (DateTime.Now - new DateTime(2000, 1, 1)).TotalMilliseconds;
        }

        #region IDeletable Members

        public bool Delete()
        {
            deleted = true;
            if (deleted)
            {
                return false;
            }
            PreDelete();
            new GenericDAO<object>(GetType()).Delete(this);
            return true;
        }

        #endregion

        protected abstract void PreDelete();
    }
}