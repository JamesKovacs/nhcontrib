namespace System.Persistence
{
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public sealed class UniqueConstraintAttribute : Attribute
	{
		public string[] ColumnNames { get; set;} 
	}
}
