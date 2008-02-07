using System;
using System.Reflection;
using NHibernate.Burrow.Exceptions;

namespace NHibernate.Burrow.Util {
    /// <summary>
    /// A loader writen to load instance of singleton types
    /// </summary>
    public class SingletonInstanceLoader {
        private const string InstancePropertyName = "Instance";

        /// <summary>
        /// Load the singleton of the type by constructor 
        /// </summary>
        /// <param name="t"></param>
        /// <returns>null if there is no public non-parameter constructor</returns>
        public static object LoadByConstrutor(System.Type t) {
            ConstructorInfo ci = t.GetConstructor(new System.Type[0]);
            if (ci == null)
                throw new Exceptions.NoSuitableContructorException(
                    t.Name + " must have a public constructor without parameter");
            return ci.Invoke(new object[0]);
        }

        /// <summary>
        /// Load the singleton by static property named "Instance"
        /// </summary>
        /// <param name="t"></param>
        /// <returns>return null if there is no such 
        /// </returns>
        public static object LoadByInstanceProperty(System.Type t) {
            PropertyInfo pi = t.GetProperty(InstancePropertyName, BindingFlags.Static | BindingFlags.Public);
            if (pi == null)
                return null;
            return pi.GetValue(null, null);
        }

        /// <summary>
        /// Load the singleton by all means
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object Load(System.Type t) {
            if (t == null)
                throw new ArgumentNullException();
            object retVal;
            retVal = LoadByInstanceProperty(t);
            if (retVal == null)
                retVal = LoadByConstrutor(t);

            if (retVal == null)
                throw new Exceptions.PropertyNotFoundException("The type " + t.Name +
                                                 " must have either a static property named \"Instance\" or a public non-parameter constructor");
            return retVal;
        }


        /// <summary>
        /// Load type by name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Load<T>(string name) {
            System.Type type = System.Type.GetType(name);
            if (type == null)
                throw new TypeNotFoundException("Type " + name + " not found.");
            return (T) Load(type);
        }

        /// <summary>
        /// Generic version of Load(System.Type t)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// <see cref="SingletonInstanceLoader.Load(System.Type t)"/>
        /// </remarks>
        public static T Load<T>() {
            return (T) Load(typeof (T));
        }

        
        /// <summary>
        /// judge if Type t is a loadable singleton
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Loadable(System.Type t) {
            PropertyInfo pi = t.GetProperty(InstancePropertyName, BindingFlags.Static | BindingFlags.Public);
            ConstructorInfo ci = t.GetConstructor(new System.Type[0]);
            return ci != null || pi != null;
        }
    }
}