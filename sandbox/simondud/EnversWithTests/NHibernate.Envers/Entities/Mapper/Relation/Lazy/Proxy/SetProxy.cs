using System;
using System.Collections.Generic;
using System.Text;
using Iesi.Collections.Generic;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Proxy
{
/**
 * @author Adam Warski (adam at warski dot org)
 */
public class SetProxy<U>: CollectionProxy<U, ISet<U>>, ISet<U> {

    public SetProxy() {
    }

    public SetProxy(IInitializor initializor):base(initializor) {
    }

    public object Clone()
    {
        throw new NotImplementedException();
    }

    public ISet<U> Union(ISet<U> a)
    {
        throw new NotImplementedException();
    }

    public ISet<U> Intersect(ISet<U> a)
    {
        throw new NotImplementedException();
    }

    public ISet<U> Minus(ISet<U> a)
    {
        throw new NotImplementedException();
    }

    public ISet<U> ExclusiveOr(ISet<U> a)
    {
        throw new NotImplementedException();
    }

    public bool ContainsAll(ICollection<U> c)
    {
        throw new NotImplementedException();
    }

    public bool Add(U o)
    {
        throw new NotImplementedException();
    }

    public bool AddAll(ICollection<U> c)
    {
        throw new NotImplementedException();
    }

    public bool RemoveAll(ICollection<U> c)
    {
        throw new NotImplementedException();
    }

    public bool RetainAll(ICollection<U> c)
    {
        throw new NotImplementedException();
    }

    public bool IsEmpty
    {
        get { throw new NotImplementedException(); }
    }
}
}
