using System;
using System.Reflection;

namespace NHibernate.Burrow.Util.DomainSession {
    /// <summary>
    /// A loader writen to load instance of singleton types
    /// </summary>
    public class Util {

        /// <summary>
        /// Load the singleton of the type by constructor 
        /// </summary>
        /// <param name="t"></param>
        /// <returns>null if there is no public non-parameter constructor</returns>
        public static object Create(string  name) {
            System.Type type = System.Type.GetType(name);
            if (type == null)
                throw new ArgumentException("Type " + name + " not found.");
            ConstructorInfo ci = type.GetConstructor(new System.Type[0]);
            if (ci == null)
                throw new NotSupportedException(
                    name + " must have a public constructor without parameter");
            return ci.Invoke(new object[0]);
        }

        
    }
}