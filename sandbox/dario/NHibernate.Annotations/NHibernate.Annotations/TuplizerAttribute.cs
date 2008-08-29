using System;

namespace NHibernate.Annotations
{
	public class TuplizerAttribute : Attribute
	{
		/// <summary>
		/// Tuplizer implementation
		/// </summary>
		public System.Type Implementation { get; set; }

		/// <summary>
		/// Either POCO or dynamic-map
		/// </summary>
		public EntityMode EntityMode { get; set; }
	}
}