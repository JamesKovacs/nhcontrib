using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor
{
    public interface IInitializor<T>
    {
        T Initialize();
    }
}
