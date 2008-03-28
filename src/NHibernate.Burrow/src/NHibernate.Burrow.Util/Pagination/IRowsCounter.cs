using NHibernate;

namespace NHibernate.Burrow.Util.Pagination
{
	/// <summary>
	/// Interface for the row counter provider.
	/// </summary>
	public interface IRowsCounter
	{
		/// <summary>
		/// Get the row count.
		/// </summary>
		/// <param name="session">The <see cref="ISession"/>.</param>
		/// <returns>The row count.</returns>
		long GetRowsCount(ISession session);
	}
}