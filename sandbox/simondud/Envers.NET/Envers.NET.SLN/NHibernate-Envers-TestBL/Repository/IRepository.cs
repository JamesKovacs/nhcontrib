using System;
using System.Collections.Generic;

namespace Envers.Net.Repository
{
    public interface IRepository<Type>
    {
        void Add(Type entity);
        void Update(Type entity);
        void Remove(Type entity);
        Type GetById(object entityId);
        ICollection<Type> GetByType(string type);
    }
}

