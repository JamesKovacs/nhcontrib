namespace System.Persistence
{
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public sealed class SecondaryTableAttribute : Attribute
	{
		public string Name { get; set; }
		public string Catalog { get; set; }
		public string Schema { get; set; }
		public PrimaryKeyJoinColumnAttribute[] PkJoinColumns { get; set; }
		public UniqueConstraintAttribute[] UniqueConstraints { get; set; }
	}
}