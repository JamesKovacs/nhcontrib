using System;
using System.Reflection;

namespace NHibernate.Annotations.Extensions
{
	public static class AttributeExtensionMethods
	{
		public static bool IsAttributePresent<T>(this System.Type clazz) where T : Attribute
		{
			return AttributeHelper.IsAttributePresent<T>(clazz);
		}

		public static bool IsAttributePresent<T>(this PropertyInfo property) where T : Attribute
		{
			return AttributeHelper.IsAttributePresent<T>(property);
		}

		public static T GetAttribute<T>(this System.Type clazz) where T : Attribute
		{ 
			return AttributeHelper.GetFirst<T>(clazz);
		}

		public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute
		{
			return AttributeHelper.GetFirst<T>(property);
		}
	}
}