using System;

namespace NHibernate.Annotations
{
	/// <summary>
	/// Parameter (basically key/value pattern)
	/// TODO: see if it's necessary
	/// </summary>
	public class ParameterAttribute : Attribute
	{
		public string Name { get; set;}
		public string Value { get; set;}
	}
}