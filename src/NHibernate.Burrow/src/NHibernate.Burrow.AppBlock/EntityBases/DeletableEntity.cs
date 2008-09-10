using System;
using NHibernate.Burrow.AppBlock.DAOBases;

namespace NHibernate.Burrow.AppBlock.EntityBases
{
    public abstract class DeletableEntity : IDeletable
    {
        private bool deleted;

        #region IDeletable Members

        public bool Delete()
        {
            if (deleted)
            {
                return false;
            }
			deleted = true;
          
            PreDelete();
            new GenericDAO<object>(GetType()).Delete(this);
            return true;
        }

        #endregion

        protected abstract void PreDelete();
    }
}