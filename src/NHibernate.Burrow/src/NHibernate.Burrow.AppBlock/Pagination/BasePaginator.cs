using System;

namespace NHibernate.Burrow.AppBlock.Pagination {
    /// <summary>
    /// General purpose paginator.
    /// </summary>
    /// <remarks>
    /// It can be useful if you think to hold the state of pagination in some kind of cache.
    /// </remarks>
    [Serializable]
    public class BasePaginator : IPaginator {
        private int? currentPageNumber;
        private int? lastPageNumber;

        public BasePaginator() {}

        /// <summary>
        /// Create a new instance of <see cref="BasePaginator"/>.
        /// </summary>
        /// <param name="lastPageNumber">The las available page.</param>
        /// <remarks>
        /// The <see cref="CurrentPageNumber"/> is set to the first available page.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="lastPageNumber"/> is less than zero.</exception>
        public BasePaginator(int lastPageNumber) {
            if (lastPageNumber < 0)
                throw new ArgumentOutOfRangeException("lastPageNumber",
                                                      string.Format("expected great than or equals zero; was {0}",
                                                                    lastPageNumber));
            LastPageNumber = lastPageNumber;
        }

        #region IPaginator Members

        /// <summary>
        /// The number of the current page.
        /// </summary>
        public int? CurrentPageNumber {
            get { return currentPageNumber; }
            protected set { currentPageNumber = value; }
        }

        /// <summary>
        /// The number of the last page.
        /// </summary>
        public virtual int? LastPageNumber {
            get { return lastPageNumber; }
            protected set {
                lastPageNumber = value;
                if (!currentPageNumber.HasValue && lastPageNumber.HasValue)
                    currentPageNumber = NextPageNumber;
            }
        }

        /// <summary>
        /// The number of the next page.
        /// </summary>
        public int NextPageNumber {
            get {
                return
                    (!LastPageNumber.HasValue)
                        ? currentPageNumber.GetValueOrDefault() + 1
                        : (currentPageNumber.GetValueOrDefault() + 1) > LastPageNumber.Value
                              ? LastPageNumber.Value
                              : currentPageNumber.GetValueOrDefault() + 1;
            }
        }

        /// <summary>
        /// The number of the previous page.
        /// </summary>
        public int PreviousPageNumber {
            get {
                return
                    ((currentPageNumber.GetValueOrDefault() - 1) < FirstPageNumber)
                        ? FirstPageNumber
                        : currentPageNumber.GetValueOrDefault() - 1;
            }
        }

        /// <summary>
        /// The number of the first page.
        /// </summary>
        public int FirstPageNumber {
            get { return (LastPageNumber.HasValue && LastPageNumber < 1) ? 0 : 1; }
        }

        /// <summary>
        /// True if has a previous page; false otherwise.
        /// </summary>
        public bool HasPrevious {
            get { return currentPageNumber > FirstPageNumber; }
        }

        /// <summary>
        /// True if has a next page; false otherwise.
        /// </summary>
        public bool HasNext {
            get { return !LastPageNumber.HasValue || currentPageNumber < LastPageNumber; }
        }

        #endregion

        /// <summary>
        /// Move the curret page to a given page number.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the <paramref name="pageNumber"/> is great
        /// then <see cref="LastPageNumber"/>.
        /// </exception>
        protected void GotoPageNumber(int pageNumber) {
            if (LastPageNumber.HasValue && pageNumber > LastPageNumber)
                throw new ArgumentOutOfRangeException("pageNumber");
            currentPageNumber = pageNumber;
        }
    }
}