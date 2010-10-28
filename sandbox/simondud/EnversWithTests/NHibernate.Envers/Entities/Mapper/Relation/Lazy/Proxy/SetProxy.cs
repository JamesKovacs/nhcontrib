using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Proxy
{
	public class SetProxy<T>: CollectionProxy<T, ISet<T>>, ISet<T> {

		public SetProxy() {
		}

		public SetProxy(IInitializor<T> initializor):base(initializor) {
		}

		public object Clone()
		{
			throw new NotImplementedException();
		}

		public ISet<T> Union(ISet<T> a)
		{
			throw new NotImplementedException();
		}

		public ISet<T> Intersect(ISet<T> a)
		{
			throw new NotImplementedException();
		}

		public ISet<T> Minus(ISet<T> a)
		{
			throw new NotImplementedException();
		}

		public ISet<T> ExclusiveOr(ISet<T> a)
		{
			throw new NotImplementedException();
		}

		public bool ContainsAll(ICollection<T> c)
		{
			throw new NotImplementedException();
		}

		public bool Add(T o)
		{
			throw new NotImplementedException();
		}

		public bool AddAll(ICollection<T> c)
		{
			throw new NotImplementedException();
		}

		public bool RemoveAll(ICollection<T> c)
		{
			throw new NotImplementedException();
		}

		public bool RetainAll(ICollection<T> c)
		{
			throw new NotImplementedException();
		}

		public bool IsEmpty
		{
			get { throw new NotImplementedException(); }
		}
	}
}
