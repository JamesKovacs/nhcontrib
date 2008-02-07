using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.DataContainers
{
    /// <summary>
    /// A data container which uses guid as key
    /// </summary>
    public class GuidDataContainer
    {
        private readonly IDictionary<Guid, object> dict = new Dictionary<Guid, object>();
        public Guid CreateSlot(object val){
            Guid gid = Guid.NewGuid();
            dict.Add(gid, val);
            return gid;
        }

        public void Set(Guid key , object  val) {
            dict[key] = val;
        }

        public object Get(Guid key) {
            return dict[key];
        } 
        public T Get<T>(Guid key) {
            return (T)dict[key];
        }  
        
        public bool TryGet<T>(Guid key, out T val) {
            object o;
            bool retVal = dict.TryGetValue(key,  out o);
            if(retVal)
                val = (T) o;
            else
                val = default(T);
            return retVal;
        }

        public bool Remove(Guid gid) {
            return dict.Remove(gid);
        }

        public bool ContainsKey(Guid gid) {
            return dict.ContainsKey(gid);
        }
    }
}
