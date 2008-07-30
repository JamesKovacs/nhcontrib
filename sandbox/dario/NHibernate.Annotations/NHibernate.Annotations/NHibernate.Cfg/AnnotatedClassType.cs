namespace NHibernate.Cfg
{
	/// <summary>
	/// Type of annotation of a class will give its type
	/// </summary>
	public enum AnnotatedClassType
	{
		/// <summary>
		/// has no revelent top level annotation
		/// </summary>
		NONE,

		/// <summary>
		/// has [Entity] annotation
		/// </summary>
		ENTITY,

		/// <summary>
		/// has a [Embeddable] annotation
		/// </summary>
		EMBEDDABLE,

		/// <summary>
		/// has [EmbeddedSuperclass] annotation
		/// </summary>
		EMBEDDABLE_SUPERCLASS
	}
}