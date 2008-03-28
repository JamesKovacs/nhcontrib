namespace NHibernate.Burrow.Util.Pagination
{
	/// <summary>
	/// Classic basic interface for any type of general purpose paginator.
	/// </summary>
	public interface IPaginator
	{
		/// <summary>
		/// The number of the current page.
		/// </summary>
		int? CurrentPageNumber { get; }

		/// <summary>
		/// The number of the last page.
		/// </summary>
		int? LastPageNumber { get; }

		/// <summary>
		/// The number of the next page.
		/// </summary>
		int NextPageNumber { get; }

		/// <summary>
		/// The number of the previous page.
		/// </summary>
		int PreviousPageNumber { get; }

		/// <summary>
		/// The number of the first page.
		/// </summary>
		int FirstPageNumber { get; }

		/// <summary>
		/// True if has a previous page; false otherwise.
		/// </summary>
		bool HasPrevious { get; }

		/// <summary>
		/// True if has a next page; false otherwise.
		/// </summary>
		bool HasNext { get; }
	}
}