using System;
using System.Collections.Generic;
using System.Reflection;

namespace NHibernate.Annotations
{
    public class AttributeHelper
    {
		public static T GetFirst<T>(ICustomAttributeProvider property) where T : Attribute
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

    	public static IList<T> GetAll<T>(ICustomAttributeProvider property) where T : Attribute
		{
			object[] attributes = property.GetCustomAttributes(typeof(T), false);
			return GetAll<T>(attributes);
		}

		private static IList<T> GetAll<T>(object[] attributes) where T : Attribute
		{
			var matches = new List<T>();
			foreach (object attribute in attributes)
			{
				matches.Add((T)attribute);
			}
			return matches;
		}

        public static bool IsAttributePresent<T>(ICustomAttributeProvider clazz) where T : Attribute
		{
			throw new NotImplementedException();
		}

    }
}
