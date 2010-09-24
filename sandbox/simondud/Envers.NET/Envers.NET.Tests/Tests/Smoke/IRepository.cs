using System;
using System.Collections;
using System.Collections.Generic;

namespace NHibernate.Envers.Tests.Smoke
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        T GetById(object entityId);
        ICollection<T> GetByType(string type);
        IList GetAllRevisionIds(DomainObject entity);
        IList GetAllRevisionIds(System.Type type, long Id);
        T GetRevision(System.Type tip, long Id, long VersionId);
    }
}

