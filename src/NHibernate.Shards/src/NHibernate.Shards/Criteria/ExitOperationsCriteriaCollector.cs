using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Shards.Strategy.Exit;

namespace NHibernate.Shards.Criteria
{
	public class ExitOperationsCriteriaCollector : IExitOperationsCollector
	{
		private int? maxResults;
		private int? firstResult;
        private readonly Distinct distinct;
		private AggregateProjection avgProjection;
		private AggregateProjection aggregateProjection;
		private RowCountProjection rowCountProjection;
		private ISessionFactoryImplementor sessionFactoryImplementor;

		private readonly IList<Order> orders = new List<Order>();

		public IExitOperationsCollector MaxResults(int maxResultsValue)
		{
			maxResults = maxResultsValue;
			return this;
		}

		public IExitOperationsCollector FirstResult(int firstResultValue)
		{
			firstResult = firstResultValue;
			return this;
		}

		public IExitOperationsCollector AddProjection(IProjection projection)
		{
			if (projection.GetType().IsAssignableFrom(distinct.GetType()))
			{
				throw new NotSupportedException();
			}
			if (projection.GetType().IsAssignableFrom(rowCountProjection.GetType()))
			{
				rowCountProjection = (RowCountProjection) projection;
			}
			if (projection.GetType().IsAssignableFrom(aggregateProjection.GetType()))
			{
				if (projection.ToString().ToLower().StartsWith("avg"))
				{
					avgProjection = (AggregateProjection) projection;
				}
				else
				{
					aggregateProjection = (AggregateProjection) projection;
				}
			}
			else
			{
				throw new NotSupportedException();
			}

			return this;
		}

		public IExitOperationsCollector AddOrder(Order order)
		{
			orders.Add(order);
			return this;
		}

		public IList Apply(IList result)
		{
			if (distinct != null)
			{
				result = new DistinctExitOperation(distinct).Apply(result);
			}
			foreach (Order order in orders)
			{
				result = new OrderExitOperation(order).Apply(result);
			}
			if (firstResult != null)
			{
				result = new FirstResultExitOperation((int) firstResult).Apply(result);
			}
			if (maxResults != null)
			{
				result = new MaxResultsExitOperation((int) maxResults).Apply(result);
			}
			ProjectionExitOperationFactory factory = ProjectionExitOperationFactory.GetFactory();

			if (rowCountProjection != null)
			{
				result = factory.GetProjectionExitOperation(rowCountProjection, sessionFactoryImplementor).Apply(result);
			}
			if (avgProjection != null)
			{
				result = new AvgResultsExitOperation().Apply(result);
			}
			if (aggregateProjection != null)
			{
				result = factory.GetProjectionExitOperation(aggregateProjection, sessionFactoryImplementor).Apply(result);
			}

			return result;
		}

		public void SetSessionFactory(ISessionFactoryImplementor sessionFactoryImplementorValue)
		{
			sessionFactoryImplementor = sessionFactoryImplementorValue;
		}
	}
}