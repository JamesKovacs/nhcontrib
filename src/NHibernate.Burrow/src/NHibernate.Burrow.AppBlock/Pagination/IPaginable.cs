using System.Collections.Generic;

namespace NHibernate.Burrow.AppBlock.Pagination {
    /// <summary>
    /// Interface for paginable results.
    /// </summary>
    /// <typeparam name="T">The type of DAO.</typeparam>
    /// <remarks>
    /// The interface was inspired from some where on the NET, but now I can't found it anymore.
    /// Even if this interface is not the same, if somebody else found it, please let me know.
    /// </remarks>
    public interface IPaginable<T> {
        /// <summary>
        /// Session getter.
        /// </summary>
        /// <returns>The <see cref="ISession"/>.</returns>
        ISession GetSession();

        /// <summary>
        /// All results without paging.
        /// </summary>
        /// <returns>The list of all instances.</returns>
        IList<T> ListAll();

        /// <summary>
        /// Page result getter.
        /// </summary>
        /// <param name="pageSize">The page's elements quantity.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>The page's elements list.</returns>
        /// <remarks>The max size of the list is <paramref name="pageSize"/>.</remarks>
        IList<T> GetPage(int pageSize, int pageNumber);
    }
}