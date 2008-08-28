using System;

namespace NHibernate.Annotations
{
	/// <summary>
	/// This specifies that a property is part of the natural id of the entity.
	/// </summary>
	class NaturalIdAttribute : Attribute
	{
		/// <summary>
		/// If this natural id component is mutable or not.
		/// </summary>
		public bool IsMutable { get; set; }
	}
}
