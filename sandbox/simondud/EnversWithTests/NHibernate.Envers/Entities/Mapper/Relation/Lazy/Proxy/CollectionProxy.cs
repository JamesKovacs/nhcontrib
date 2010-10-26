using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Proxy
{
    /**
     * @author Adam Warski (adam at warski dot org)
     */
    //T extends Collection<U>
    [Serializable]
    public abstract class CollectionProxy<U, T> : ICollection<U> {

	    private readonly IInitializor initializor;
        protected T delegat;

        protected CollectionProxy() 
		{
        }

        public CollectionProxy(IInitializor initializor) 
		{
            this.initializor = initializor;
        }

        protected void CheckInit() {
            if (delegat == null) {
                delegat = (T)initializor.Initialize();
            }
        }

        public int Count 
		{
			get 
			{
				CheckInit();
				return ((ICollection<U>)delegat).Count;
			}
		}

        public bool IsReadOnly
        {
            get
            {
                CheckInit();
                return ((ICollection<U>)delegat).IsReadOnly;
            }
        }

        public bool Contains(U o) 
		{
            CheckInit();
            return ((ICollection<U>)delegat).Contains<U>(o);
        }

        public void CopyTo(U[] array, int arrayIndex)
        {
            CheckInit();
            ((ICollection<U>)delegat).CopyTo(array, arrayIndex);
        }

        public IEnumerator<U> GetEnumerator() 
		{
            CheckInit();
            return ((ICollection<U>)delegat).GetEnumerator();
        }

         public void Add(U o) 
		 {
            CheckInit();
            ((ICollection<U>)delegat).Add(o);
        }

        public bool Remove(U o) 
		{
            CheckInit();
            return ((ICollection<U>)delegat).Remove(o);
        }

        public void Clear() 
		{
            CheckInit();
            ((ICollection<U>)delegat).Clear();
        }

        public override string ToString() 
		{
            CheckInit();
            return delegat.ToString();
        }

        public override bool Equals(object obj) 
		{
            CheckInit();
            return delegat.Equals(obj);
        }

        public override int GetHashCode() 
		{
            CheckInit();
            return delegat.GetHashCode();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
