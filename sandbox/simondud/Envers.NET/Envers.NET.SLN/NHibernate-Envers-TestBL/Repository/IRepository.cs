using System;
using System.Collections;
using System.Collections.Generic;
using Envers.Net.Model;

namespace Envers.Net.Repository
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        T GetById(object entityId);
        ICollection<T> GetByType(string type);
        IList GetAllRevisionIds(DomainObject entity);
        IList<long> GetRevision(System.Type tip, long Id, long VersionId);
    }
}

