using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;

namespace NHibernate.Tool.Db2hbm
{
    public static class TypeFactory
    {
        static ILog logger = LogManager.GetLogger("db2hbm");
        public static T Create<T>(string typeName) where T:class
        {
            System.Type t = System.Type.GetType(typeName);
            if (null == t)
            {
                throw new Exception("Cannot find type:" + typeName);
            }
            var o = Activator.CreateInstance(t);
            T ret = o as T;
            if (null == ret)
            {
                throw new Exception(string.Format("Type:{0} cannot be used as {1}",t.Name,typeof(T).Name));
            }
            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (pi.CanWrite && pi.PropertyType.IsAssignableFrom(typeof(ILog)))
                {
                    pi.SetValue(ret, logger, null);
                }
            }
            return ret;
        }
    }
}
