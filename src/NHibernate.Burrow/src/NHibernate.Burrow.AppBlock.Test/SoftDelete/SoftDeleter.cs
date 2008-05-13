using NHibernate.Burrow.AppBlock.SoftDelete;
using System;

namespace NHibernate.Burrow.AppBlock.Test.SoftDelete
{
    public class SoftDeleter : ISoftDelete
    {
        private int id;
        private DateTime? deleteDate;
        private bool deleted;

        public virtual int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        public virtual DateTime? DeleteDate
        {
            get { return deleteDate; }
            set { deleteDate = value; }
        }

        public virtual bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
    }
}
