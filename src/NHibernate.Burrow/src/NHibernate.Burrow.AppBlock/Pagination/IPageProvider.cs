using System.Collections.Generic;

namespace NHibernate.Burrow.AppBlock.Pagination
{
    /// <summary>
    /// Interface for pages provider.
    /// </summary>
    /// <typeparam name="T">The type of each row of the page.</typeparam>
    /// <seealso cref="IPaginator"/>
    public interface IPageProvider<T> : IPaginator
    {
        /// <summary>
        /// Number of visible objects of each page.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// The total rows count.
        /// </summary>
        long? RowsCount { get; }

        /// <summary>
        /// Get True if the paginator has query results. False in other case.
        /// </summary>
        bool HasPages { get; }

        /// <summary>
        /// Get the list of objects for a given page number.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>The list of objects.</returns>
        IList<T> GetPage(int pageNumber);

        /// <summary>
        /// Get the list of objects of the first page.
        /// </summary>
        /// <returns>The list of objects.</returns>
        IList<T> GetFirstPage();

        /// <summary>
        /// Get the list of objects of the last page.
        /// </summary>
        /// <returns>The list of objects.</returns>
        IList<T> GetLastPage();

        /// <summary>
        /// Get the list of objects of the next page.
        /// </summary>
        /// <returns>The list of objects.</returns>
        IList<T> GetNextPage();

        /// <summary>
        /// Get the list of objects of the previous page.
        /// </summary>
        /// <returns>The list of objects.</returns>
        IList<T> GetPreviousPage();

        /// <summary>
        /// Get the list of objects of the current page.
        /// </summary>
        /// <returns>The list of objects.</returns>
        IList<T> GetCurrentPage();
    }
}