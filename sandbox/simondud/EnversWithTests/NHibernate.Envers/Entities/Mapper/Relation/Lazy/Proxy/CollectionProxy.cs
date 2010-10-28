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

	    private readonly IInitializor<U> initializor;
        protected ICollection<U> delegat;

        protected CollectionProxy() 
		{
        }

        public CollectionProxy(IInitializor<U> initializor) 
		{
            this.initializor = initializor;
        }

        protected void CheckInit() 
		{
            if (delegat == null) 
			{
                delegat = initializor.Initialize();
            }
        }

        public int Count 
		{
			get 
			{
				CheckInit();
				return delegat.Count;
			}
		}

        public bool IsReadOnly
        {
            get
            {
                CheckInit();
                return delegat.IsReadOnly;
            }
        }

        public bool Contains(U o) 
		{
            CheckInit();
            return delegat.Contains<U>(o);
        }

        public void CopyTo(U[] array, int arrayIndex)
        {
            CheckInit();
            delegat.CopyTo(array, arrayIndex);
        }

        public IEnumerator<U> GetEnumerator() 
		{
            CheckInit();
            return delegat.GetEnumerator();
        }

         public void Add(U o) 
		 {
            CheckInit();
            delegat.Add(o);
        }

        public bool Remove(U o) 
		{
            CheckInit();
            return delegat.Remove(o);
        }

        public void Clear() 
		{
            CheckInit();
            delegat.Clear();
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
