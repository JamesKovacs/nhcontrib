using System;

namespace NHibernate.Annotations
{
	/// <summary>
	/// Whether or not update entity's version on property's change
	/// If the annotation is not present, the property is involved in the optimistic lock srategy (default)
	/// </summary>
	public class OptimisticLockAttribute : Attribute
	{
		/// <summary>
		/// If true, the annotated property change will not trigger a version upgrade
		/// </summary>
		public bool IsExcluded { get; set; }
	}
}