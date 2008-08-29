using System;
using System.Collections.Generic;
using System.Reflection;

namespace NHibernate.Annotations.Extensions
{
	public static class AttributeExtensionMethods
	{
		public static bool IsAttributePresent<T>(this ICustomAttributeProvider property) where T : Attribute
		{
			return AttributeHelper.IsAttributePresent<T>(property);
		}

		public static T GetAttribute<T>(this ICustomAttributeProvider clazz) where T : Attribute
		{ 
			return AttributeHelper.GetFirst<T>(clazz);
		}

		public static IList<T> GetAttributes<T>(this System.Type clazz) where T : Attribute
		{
			return AttributeHelper.GetAll<T>(clazz);
		}
	}
}