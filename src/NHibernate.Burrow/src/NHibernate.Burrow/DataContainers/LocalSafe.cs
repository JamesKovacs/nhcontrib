using System;
using System.Collections;
using System.Web;
using System.Runtime.Remoting.Messaging;


namespace NHibernate.Burrow.DataContainers
{
    /// <summary>
    /// This storage wrapper can be used as a static field and will garuntee localness - either HttpContext local if in a HttpContext environment or ThreadLocal otherwise 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LocalSafe<T>
    {
        private const string CallContextKey = "NHibernate.Burrow.DataContainers.LocalSafeKey";
        private IDictionary callContextDictionary
        {
            get { return CallContext.GetData(CallContextKey) as IDictionary; }
            set { CallContext.SetData(CallContextKey, value); }
        }

        private readonly Guid gid = Guid.NewGuid();

        private IDictionary Container
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Items;
                }
                if (callContextDictionary == null)
                {
                    callContextDictionary = new Hashtable();
                }
                return callContextDictionary;
            }
        }

        public T Value
        {
            get
            {
                if (Container.Contains(gid))
                {
                    return (T) Container[gid];
                }
                else
                {
                    return default(T);
                }
            }
            set { Container[gid] = value; }
        }
    }
}