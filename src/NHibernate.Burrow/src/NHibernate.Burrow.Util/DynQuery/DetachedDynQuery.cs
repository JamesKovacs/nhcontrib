using System;
using NHibernate;
using NHibernate.Burrow.Util.NH;
using NHibernate.Burrow.Util.NH.Impl;

namespace NHibernate.Burrow.Util.DynQuery
{
	[Serializable]
	public class DetachedDynQuery : AbstractDetachedQuery
	{
		private readonly From from;
		private readonly Select select;

		public DetachedDynQuery(Select select)
		{
			if (select == null)
				throw new ArgumentNullException("select");

			this.select = select;
			from = select.From();
		}

		public DetachedDynQuery(From from)
		{
			if (from == null)
				throw new ArgumentNullException("from");

			this.from = from;
		}

		public string Hql
		{
			get { return (select != null) ? select.Clause : from.Clause; }
		}

		/// <summary>
		/// Get an executable instance of <see cref="IQuery"/>, to actually run the query.
		/// </summary>
		public override IQuery GetExecutableQuery(ISession session)
		{
			IQuery result = session.CreateQuery((select != null) ? select.Clause : from.Clause);
			SetQueryProperties(result);
			return result;
		}

		public IDetachedQuery TransformToRowCount()
		{
			Select s = new Select("count(*)");
			s.SetFrom(from.FromWhereClause());
			DetachedQuery result = new DetachedQuery(s.Clause);
			result.CopyParametersFrom(this);
			return result;
		}
	}
}
