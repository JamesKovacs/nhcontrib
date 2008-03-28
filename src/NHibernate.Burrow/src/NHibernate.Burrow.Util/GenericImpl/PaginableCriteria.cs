using System;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Burrow.Util.Pagination;
using NHibernate.Engine;

namespace NHibernate.Burrow.Util.GenericImpl
{
	/// <summary>
	/// Generic implementation of <see cref="IPaginable{T}"/> based on <see cref="DetachedCriteria"/>.
	/// </summary>
	/// <typeparam name="T">The type of DAO.</typeparam>
	/// <seealso cref="NHibernate.Criterion.DetachedCriteria"/>
	public class PaginableCriteria<T> : AbstractPaginableCriteria<T>, IRowsCounter
	{
		private readonly ISession session;
		private readonly DetachedCriteria detachedCriteria;
		private readonly DetachedCriteria counterCriteria;

		/// <summary>
		/// Create a new instance of <see cref="PaginableCriteria{T}"/>.
		/// </summary>
		/// <param name="session">The session (may be the same session of the DAO).</param>
		/// <param name="detachedCriteria">The detached criteria.</param>
		public PaginableCriteria(ISession session, DetachedCriteria detachedCriteria)
		{
			if (session == null)
				throw new ArgumentNullException("session");

			if (detachedCriteria == null)
				throw new ArgumentNullException("detachedCriteria");

			this.session = session;
			this.detachedCriteria = detachedCriteria;
			counterCriteria = TransformToRowCount(detachedCriteria);
		}
		
		protected override DetachedCriteria Criteria
		{
			get { return detachedCriteria; }
		}

		public override ISession GetSession()
		{
			return session;
		}

		#region IRowsCounter Members

		public long GetRowsCount(ISession session)
		{
			return counterCriteria.GetExecutableCriteria(session).UniqueResult<long>();
		}

		private static DetachedCriteria TransformToRowCount(DetachedCriteria criteria)
		{
			DetachedCriteria cloned = CriteriaTransformer.Clone(criteria);
			cloned.Orders.Clear();
			cloned
				.SetResultTransformer(null)
				.SetFirstResult(0)
				.SetMaxResults(RowSelection.NoValue)
				.SetProjection(Projections.RowCountInt64());
			return cloned;

		}
		#endregion
	}
}