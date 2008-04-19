using NHibernate.Burrow.AppBlock.DAOBases;
using NHibernate.Burrow.AppBlock.EntityBases;
using NHibernate.Criterion;

namespace NHibernate.Burrow.AppBlock.Test.EntityBases
{
    public class HashIdMockClass : EntityWHashIdBase
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        protected override void PreDelete()
        {
            // do nothing
        }

        public void Save()
        {
            new HashIdMockClassDAO().Save(this);
        }
    }

    public class HashIdMockClassDAO : GenericDAO<HashIdMockClass>
    {
        public HashIdMockClass FindByName(string name)
        {
            return CreateCriteria().Add(Expression.Eq("Name", name)).UniqueResult<HashIdMockClass>();
        }
    }
}