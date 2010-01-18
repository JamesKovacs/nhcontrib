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
        static Dictionary<System.Type, object> services = new Dictionary<System.Type, object>();   
        public static void RegisterServiceInstance<T>(T instance)
        {
            services[typeof(T)] = instance;
        }
        static ILog logger = LogManager.GetLogger("db2hbm");
        public static T Create<T>(string typeName) where T:class
        {
            System.Type t = System.Type.GetType(typeName);
            if (null == t)
            {
                throw new Exception("Cannot find type:" + typeName);
            }
            
            var o = Activator.CreateInstance(t);
            T  ret = o as T;
            if (null == ret)
            {
                throw new Exception(string.Format("Type:{0} cannot be used as {1}",t.Name,typeof(T).Name));
            }
            
            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (pi.CanWrite )
                {
                    if( pi.PropertyType.IsAssignableFrom(typeof(ILog)) )
                        pi.SetValue(ret, logger, null);
                    if (services.ContainsKey(pi.PropertyType))
                        pi.SetValue(ret, services[pi.PropertyType], null);
                }
                
            }
            return ret;
        }
        public static T Create<T>(cfg.classref config) where T : class
        {
            T instance = Create<T>(config.@class);
            PropertyInfo customScript = null;
            if (null != config.property)
            {
                foreach (var v in config.property)
                {
                    PropertyInfo pi = instance.GetType().GetProperty(v.name);
                    if (pi.GetCustomAttributes(typeof(BooDecoratorAttribute), true).Length > 0)
                    {
                        if (customScript != null)
                            throw new Exception("Too many property specify a boo derivative of class:" + typeof(T).Name);
                        customScript = pi;
                    }
                    if (pi == null || !pi.CanWrite)
                        throw new Exception(string.Format("Can't configure type {0}: property {1} does not exists or is read-only", typeof(T).Name, v.name));
                    pi.SetValue(instance, Convert.ChangeType(v.Value, pi.PropertyType), null);
                }
            }
            if (null != customScript)
            {
                string script = customScript.GetValue(instance, null) as string;
                if( !string.IsNullOrEmpty(script) )
                    instance = BooActivator.CreateInstance<T>(script);
            }
            return instance;
        }
    }
}
