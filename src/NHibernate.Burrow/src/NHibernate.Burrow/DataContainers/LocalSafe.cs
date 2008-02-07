using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace NHibernate.Burrow.DataContainers
{
    /// <summary>
    /// This storage wrapper can be used as a static field and will garuntee localness - either HttpContext local if in a HttpContext environment or ThreadLocal otherwise 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class  LocalSafe<T> 
    {
        [ThreadStatic]
        private static IDictionary threadLocalDictionary;  

        private IDictionary Container {
            get {
                if (HttpContext.Current != null)
                    return HttpContext.Current.Items;
                if (threadLocalDictionary == null)
                    threadLocalDictionary = new Hashtable();
                return threadLocalDictionary;
            }
        }

        readonly Guid gid = Guid.NewGuid();
        
        public T Value {
            get {
                if (Container.Contains(gid))
                    return (T)Container[gid];
                else return default(T);
            }
            set {
                Container[gid] = value;
            }
        }
    }
}
