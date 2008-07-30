namespace System.Persistence
{
	public enum OptimisticLockType
	{
		/// <summary>
		/// no optimistic locking
		/// </summary>
		None,
		/// <summary>
		/// use a column version
		/// </summary>
		Version,
		/// <summary>
		/// dirty columns are compared
		/// </summary>
		Dirty,
		/// <summary>
		/// all columns are compared
		/// </summary>
		All
	}
}