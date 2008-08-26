using System;

namespace NHibernate.Annotations
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class TypeAttribute : Attribute
	{
		public string Type { get; set; }
		public ParameterAttribute[] Parameters { get; set; }
	}
}