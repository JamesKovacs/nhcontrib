using System;
using System.Reflection;
using NHibernate;

namespace NHibernate.Burrow.NHDomain {
    /// <summary>
    /// A generic helper loader for loading persistant object by Id
    /// </summary>
    public class Loader  {
        private static readonly Loader instance = new Loader();

        /// <summary>
        /// a instance of the loader
        /// </summary>
        public static Loader Instance {
            get { return instance; }
        }

        #region ILoader Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Load(System.Type t, object id) {
            return GetSession(t)
                .Get(t, id);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Get(System.Type t, object id) {
            return GetSession(t)
                .Get(t, id);
        }

        public object GetId(object o) {
            if (o == null)
                return null;
            PropertyInfo pi = o.GetType().GetProperty("Id");
            if(pi != null)
                return pi.GetValue(o, null);
            else
                return GetSession(o.GetType()).GetIdentifier(o);
        }

        private static ISession GetSession(System.Type t) {
            return SessionManager.GetInstance(t).GetSession();
        }
    }
}