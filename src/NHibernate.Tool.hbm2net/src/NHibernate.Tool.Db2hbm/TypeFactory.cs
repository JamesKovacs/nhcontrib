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
            Inject( ret);
            return ret;
        }

        private static void Inject( object instance) 
        {
            var t = instance.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (pi.CanWrite)
                {
                    if (pi.PropertyType.IsAssignableFrom(typeof(ILog)))
                        pi.SetValue(instance, logger, null);
                    if (services.ContainsKey(pi.PropertyType))
                        pi.SetValue(instance, services[pi.PropertyType], null);
                }
            }
        }
        public static T Create<T>(cfg.classref config) where T : class
        {
            T instance = Create<T>(config.@class);
            PropertyInfo customScriptProperty = null;
            customScriptProperty = SetParams(config, instance);
            /*
            if (null != customScriptProperty)
            {
                string script = customScriptProperty.GetValue(instance, null) as string;
                if (!string.IsNullOrEmpty(script))
                {
                    instance = BooActivator.CreateInstance<T>(script);
                    Inject(instance);
                    SetParams(config, instance);
                }
            }*/
            return instance;
        }

        private static PropertyInfo SetParams(cfg.classref config, object instance)
        {
            PropertyInfo customScriptProperty = null;
            if (null != config.property)
            {
                foreach (var v in config.property)
                {
                    PropertyInfo pi = instance.GetType().GetProperty(v.name);
                    if (pi.GetCustomAttributes(typeof(BooDecoratorAttribute), true).Length > 0)
                    {
                        if (customScriptProperty != null)
                            throw new Exception("Too many property specify a boo derivative of class:" + instance.GetType().Name);
                        customScriptProperty = pi;
                    }
                    if (pi == null || !pi.CanWrite)
                        throw new Exception(string.Format("Can't configure type {0}: property {1} does not exists or is read-only", instance.GetType().Name, v.name));
                    pi.SetValue(instance, Convert.ChangeType(v.Value, pi.PropertyType), null);
                }
            }
            return customScriptProperty;
        }
    }
}
