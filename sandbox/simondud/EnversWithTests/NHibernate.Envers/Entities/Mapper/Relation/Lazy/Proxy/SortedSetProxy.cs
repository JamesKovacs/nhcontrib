using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using NHibernate.Envers.Compatibility;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Proxy
{
/**
 * @author Adam Warski (adam at warski dot org)
 * TODO Simon: de finalizat clasa
 */
public class SortedSetProxy<U> : CollectionProxy<U, SortedSet<U>>, ISortedSet<U>  {

    public SortedSetProxy(){
        if (!typeof(U).IsSubclassOf(typeof(ISortedSet<>)))
            throw new NotSupportedException("Type U has to be a subclass of ISortedSet<>");
    }

    public SortedSetProxy(IInitializor<SortedSet<U>> initializor)
    :base(initializor)
    {
        if (!typeof(U).IsSubclassOf(typeof(ISortedSet<>)))
            throw new NotSupportedException("Type U has to be a subclass of ISortedSet<>");
    }

    //public IComparer<U> GetComparer() {
    //    CheckInit();
    //    return delegat.gecomparator();
    //}

    public ISortedSet<U> SubSet(U u, U u1) {
        CheckInit();
        return ((ISortedSet<U>)delegat).SubSet(u, u1);
    }

    public ISortedSet<U> HeadSet(U u) {
        CheckInit();
        return ((ISortedSet<U>)delegat).HeadSet(u);
    }

    public ISortedSet<U> TailSet(U u) {
        CheckInit();
        return ((ISortedSet<U>)delegat).TailSet(u);
    }

    public U First() {
        CheckInit();
        return ((ISortedSet<U>)delegat).First();
    }

    public U Last() {
        CheckInit();
        return ((ISortedSet<U>)delegat).Last();
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
