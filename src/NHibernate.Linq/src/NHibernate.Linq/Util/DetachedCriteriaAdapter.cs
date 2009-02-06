using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace NHibernate.Linq.Util
{
	public static class DetachedCriteriaExtensions
	{
		public static ICriteria Adapt(this DetachedCriteria criteria, ISession session)
		{
			if (criteria == null) return null;
			return new DetachedCriteriaAdapter(criteria, session);
		}
	}

	public class DetachedCriteriaAdapter : ICriteria
	{
		private readonly DetachedCriteria _detachedCriteria;
		private readonly ISession _session;

		public DetachedCriteriaAdapter(DetachedCriteria detachedCriteria, ISession session)
		{
			_detachedCriteria = detachedCriteria;
			_session = session;
		}

		public DetachedCriteria DetachedCriteria
		{
			get { return _detachedCriteria; }
		}

		public ISession Session
		{
			get { return _session; }
		}

		#region ICriteria Members

		public IProjection Projection
		{
			get
			{
				return null;
			}
		}
		public ICriteria Add(ICriterion expression)
		{
			return _detachedCriteria.Add(expression).Adapt(_session);
		}

		public ICriteria AddOrder(Order order)
		{
			return _detachedCriteria.AddOrder(order).Adapt(_session);
		}

		public string Alias
		{
			get { return _detachedCriteria.Alias; }
		}

		public void ClearOrderds()
		{
			throw new NotSupportedException();
		}

		public ICriteria CreateAlias(string associationPath, string alias, JoinType joinType)
		{
			return _detachedCriteria.CreateAlias(associationPath, alias, joinType).Adapt(_session);
		}

		public ICriteria CreateAlias(string associationPath, string alias)
		{
			return _detachedCriteria.CreateAlias(associationPath, alias).Adapt(_session);
		}

		public ICriteria CreateCriteria(string associationPath, string alias, JoinType joinType)
		{
			return _detachedCriteria.CreateCriteria(associationPath, alias, joinType).Adapt(_session);
		}

		public ICriteria CreateCriteria(string associationPath, string alias)
		{
			return _detachedCriteria.CreateCriteria(associationPath, alias).Adapt(_session);
		}

		public ICriteria CreateCriteria(string associationPath, JoinType joinType)
		{
			return _detachedCriteria.CreateCriteria(associationPath, joinType).Adapt(_session);
		}

		public ICriteria CreateCriteria(string associationPath)
		{
			return _detachedCriteria.CreateCriteria(associationPath).Adapt(_session);
		}

		public ICriteria GetCriteriaByAlias(string alias)
		{
			return _detachedCriteria.GetCriteriaByAlias(alias).Adapt(_session);
		}

		public ICriteria GetCriteriaByPath(string path)
		{
			return _detachedCriteria.GetCriteriaByPath(path).Adapt(_session);
		}

		public IList<T> List<T>()
		{
			throw new NotSupportedException();
		}

		public void List(IList results)
		{
			throw new NotSupportedException();
		}

		public IList List()
		{
			throw new NotSupportedException();
		}

		public ICriteria SetCacheMode(CacheMode cacheMode)
		{
			return _detachedCriteria.SetCacheMode(cacheMode).Adapt(_session);
		}

		public ICriteria SetCacheRegion(string cacheRegion)
		{
			throw new NotSupportedException();
		}

		public ICriteria SetCacheable(bool cacheable)
		{
			throw new NotSupportedException();
		}

		public ICriteria SetComment(string comment)
		{
			throw new NotSupportedException();
		}

		public ICriteria SetFetchMode(string associationPath, FetchMode mode)
		{
			return _detachedCriteria.SetFetchMode(associationPath, mode).Adapt(_session);
		}

		public ICriteria SetFetchSize(int fetchSize)
		{
			throw new NotSupportedException();
		}

		public ICriteria SetFirstResult(int firstResult)
		{
			return _detachedCriteria.SetFirstResult(firstResult).Adapt(_session);
		}

		public ICriteria SetFlushMode(FlushMode flushMode)
		{
			throw new NotSupportedException();
		}

		public ICriteria SetLockMode(string alias, LockMode lockMode)
		{
			throw new NotSupportedException();
		}

		public ICriteria SetLockMode(LockMode lockMode)
		{
			throw new NotSupportedException();
		}

		public ICriteria SetMaxResults(int maxResults)
		{
			return _detachedCriteria.SetMaxResults(maxResults).Adapt(_session);
		}

		public ICriteria SetProjection(IProjection projection)
		{
			return _detachedCriteria.SetProjection(projection).Adapt(_session);
		}

		public ICriteria SetResultTransformer(IResultTransformer resultTransformer)
		{
			return _detachedCriteria.SetResultTransformer(resultTransformer).Adapt(_session);
		}

		public ICriteria SetTimeout(int timeout)
		{
			throw new NotSupportedException();
		}

		public T UniqueResult<T>()
		{
			throw new NotSupportedException();
		}

		public object UniqueResult()
		{
			throw new NotSupportedException();
		}

		public System.Type GetRootEntityTypeIfAvailable()
		{
			return _detachedCriteria.GetRootEntityTypeIfAvailable();
		}

		public void ClearOrders()
		{
			_detachedCriteria.ClearOrders();
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			throw new NotSupportedException();
		}

		#endregion
	}
}
