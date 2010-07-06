using System;
using System.Collections.Generic;

namespace Envers.Net.Repository
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        T GetById(object entityId);
        ICollection<T> GetByType(string type);
        IList<long> GetAllRevisionIds(System.Type tip);
        IList<long> GetRevision(System.Type tip, long Id, long VersionId);
    }
}

