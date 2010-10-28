using System.Collections.Generic;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor
{
    public interface IInitializor<T> 
    {
        ICollection<T> Initialize();
    }
}
