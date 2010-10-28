using System.Collections.Generic;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Proxy
{
	public class ListProxy<U> : CollectionProxy<U, IList<U>>, IList<U>
	{
		public ListProxy()
		{
		}

		public ListProxy(IInitializor<U> initializor):base(initializor) 
		{
		}

		public int IndexOf(U item)
		{
			CheckInit();
			return ((IList<U>)delegat).IndexOf(item);
		}

		public void Insert(int index, U item)
		{
			CheckInit();
			((IList<U>)delegat).Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			CheckInit();
			((IList<U>)delegat).RemoveAt(index);
		}

		public U this[int index]
		{
			get
			{
				CheckInit();
				return ((IList<U>)delegat)[index];
			}
			set
			{
				CheckInit();
				((IList<U>)delegat)[index] = value;
			}
		}
	}
}