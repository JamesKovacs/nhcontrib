using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Burrow.NHDomain;
using NHibernate.Expression;

namespace NHibernate.Burrow.Test.PersistantTests
{
    public class HashIdMockClass : NHibernate.Burrow.NHDomain.ObjWHashIdNDAOBase
    {
        private string name;

        public string Name {
            get { return name; }
            set { name = value; }
        }
        
    }

    public class HashIdMockClassDAO : GenericDAOBase<HashIdMockClass>
    {
        public HashIdMockClass FindByName(string name)
        {
            return GetCriteria().Add(NHibernate.Expression.Expression.Eq("Name", name))
                                    .UniqueResult <HashIdMockClass>();
        }
    }
}
