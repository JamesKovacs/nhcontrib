namespace NHibernate.Burrow.Util.Hql.Gold
{
	using System;
	using System.Collections.Generic;
	using NHibernate;
	using NHibernate.Engine;
	using NHibernate.Hql;

	public class QueryTranslatorFactory : IQueryTranslatorFactory
	{
		#region IQueryTranslatorFactory Members

		/// <summary>
		/// Construct a <see cref="T:NHibernate.Hql.IQueryTranslator"/> instance
		/// capable of translating an HQL query string.
		/// </summary>
		/// <param name="queryIdentifier">The query-identifier (used in <see cref="T:NHibernate.Stat.QueryStatistics"/> collection).
		/// This is typically the same as the queryString parameter except for the case of
		/// split polymorphic queries which result in multiple physical sql queries.</param>
		/// <param name="queryString">The query string to be translated</param>
		/// <param name="filters">Currently enabled filters</param>
		/// <param name="factory">The session factory</param>
		/// <returns>An appropriate translator.</returns>
		public IQueryTranslator CreateQueryTranslator(string queryIdentifier, string queryString,
		                                              IDictionary<string, IFilter> filters, ISessionFactoryImplementor factory)
		{
			return new QueryTranslator(queryIdentifier, queryString, filters, factory);
		}

		/// <summary>
		/// Construct a <see cref="T:NHibernate.Hql.IFilterTranslator"/> instance capable of
		/// translating an HQL filter string.
		/// </summary>
		/// <param name="queryIdentifier">The query-identifier (used in <see cref="T:NHibernate.Stat.QueryStatistics"/> collection).
		/// This is typically the same as the queryString parameter except for the case of
		/// split polymorphic queries which result in multiple physical sql queries.</param>
		/// <param name="queryString">The query string to be translated</param>
		/// <param name="filters">Currently enabled filters</param>
		/// <param name="factory">The session factory</param>
		/// <returns>An appropriate translator.</returns>
		public IFilterTranslator CreateFilterTranslator(string queryIdentifier, string queryString,
		                                                IDictionary<string, IFilter> filters, ISessionFactoryImplementor factory)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}