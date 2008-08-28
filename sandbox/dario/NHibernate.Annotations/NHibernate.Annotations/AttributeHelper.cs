using System;
using System.Reflection;

namespace NHibernate.Annotations
{
    public class AttributeHelper
    {
        public static T GetFirst<T>(System.Type clazz) where T : Attribute
        {
            object[] attributes = clazz.GetCustomAttributes(typeof (T), false);
        	return GetFirst<T>(attributes);
        }

		public static T GetFirst<T>(PropertyInfo property) where T : Attribute
		{
			object[] attributes = property.GetCustomAttributes(typeof(T), false);
			return GetFirst<T>(attributes);
		}

    	private static T GetFirst<T>(object[] attributes)  where T : Attribute
		{
			if (attributes == null) return default(T);

			if (attributes.Length > 0)
				return (T)attributes[0];

			return default(T);
		}

        public static bool IsAttributePresent<T>(System.Type clazz) where T : Attribute
        {
            throw new NotImplementedException();
        }

		public static bool IsAttributePresent<T>(PropertyInfo clazz) where T : Attribute
		{
			throw new NotImplementedException();
		}

    }
}
