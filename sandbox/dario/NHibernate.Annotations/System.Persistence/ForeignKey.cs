namespace System.Persistence
{
	/// <summary>
	/// Define the foreign key name
	/// </summary>
	public class ForeignKeyAttribute : Attribute
	{
		/// <summary>
		/// Name of the foreign key.  Used in OneToMany, ManyToOne, and OneToOne
		/// relationships.  Used for the owning side in ManyToMany relationships
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Used for the non-owning side of a ManyToMany relationship.  Ignored
		/// in other relationships
		/// </summary>
		public string InverseName { get; set; }
	}
}