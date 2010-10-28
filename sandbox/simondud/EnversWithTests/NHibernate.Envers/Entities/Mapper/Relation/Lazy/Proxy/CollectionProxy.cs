using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Proxy
{
    [Serializable]
    public abstract class CollectionProxy<T, TDelegate> : ICollection<T>
										where TDelegate : class, ICollection<T>
	{
		private readonly IInitializor<T> initializor;
		protected TDelegate delegat;

        protected CollectionProxy() 
		{
        }

    	protected CollectionProxy(IInitializor<T> initializor) 
		{
            this.initializor = initializor;
        }

        protected void CheckInit() 
		{
            if (delegat == null)
            {
                delegat = (TDelegate) initializor.Initialize();
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

        public bool Contains(T o) 
		{
            CheckInit();
            return delegat.Contains<T>(o);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CheckInit();
            delegat.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() 
		{
            CheckInit();
            return delegat.GetEnumerator();
        }

         public void Add(T o) 
		 {
            CheckInit();
            delegat.Add(o);
        }

        public bool Remove(T o) 
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
