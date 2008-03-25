using System;
using NHibernate.Burrow.Util.NH.Impl;
using NHibernate.Burrow.Util.Pagination;

namespace NHibernate.Burrow.Util.GenericImpl
{
    /// <summary>
    /// Generic implementation of <see cref="IPaginable{T}"/> based on <see cref="IDetachedQuery"/>.
    /// </summary>
    /// <typeparam name="T">The type of DAO.</typeparam>
    /// <seealso cref="IDetachedQuery"/>
    /// <seealso cref="NH.Impl.DetachedQuery"/>
    /// <seealso cref="DetachedNamedQuery"/>
    public class PaginableQuery<T> : AbstractPaginableQuery<T>
    {
        private readonly IDetachedQuery detachedQuery;
        private readonly ISession session;

        /// <summary>
        /// Create a new instance of <see cref="PaginableQuery{T}"/>.
        /// </summary>
        /// <param name="session">The session (may be the same session of the DAO).</param>
        /// <param name="detachedQuery">The detached query.</param>
        public PaginableQuery(ISession session, IDetachedQuery detachedQuery)
        {
            if (detachedQuery == null)
            {
                throw new ArgumentNullException("detachedQuery");
            }
            this.session = session;
            this.detachedQuery = detachedQuery;
        }

        protected override IDetachedQuery DetachedQuery
        {
            get { return detachedQuery; }
        }

        public override ISession GetSession()
        {
            return session;
        }
    }
}