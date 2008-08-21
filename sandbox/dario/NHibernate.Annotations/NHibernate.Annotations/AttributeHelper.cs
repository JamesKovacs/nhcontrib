using System;

namespace NHibernate.Annotations
{
    public class AttributeHelper
    {
        public static T GetFirst<T>(System.Type clazz) where T : Attribute
        {
            object[] attributes = clazz.GetCustomAttributes(typeof (T), false);
            
            if (attributes == null) return default(T);
            
            if (attributes.Length > 0)
                return (T) attributes[0];
            
            return default(T);
        }
    }
}
