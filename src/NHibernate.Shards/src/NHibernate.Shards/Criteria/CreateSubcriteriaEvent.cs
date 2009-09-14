using System.Collections.Generic;

namespace NHibernate.Shards.Criteria
{
	public class CreateSubcriteriaEvent : ICriteriaEvent
	{
		private readonly ISubcriteriaFactory subcriteriaFactory;
		private readonly ISubcriteriaRegistrar subCriteriaRegistrar;
		private readonly IDictionary<IShard, ICriteria> shardToCriteriaMap;
		private readonly IDictionary<IShard, IList<ICriteriaEvent>> shardToCriteriaEventListMap;

		public CreateSubcriteriaEvent(ISubcriteriaFactory subCriteriaFactory, ISubcriteriaRegistrar subCriteriaRegistrar,
		                              IDictionary<IShard, ICriteria> shardToCriteriaMap,
		                              IDictionary<IShard, IList<ICriteriaEvent>> shardToCriteriaEventListMap)
		{
			subcriteriaFactory = subCriteriaFactory;
			this.subCriteriaRegistrar = subCriteriaRegistrar;
			this.shardToCriteriaMap = shardToCriteriaMap;
			this.shardToCriteriaEventListMap = shardToCriteriaEventListMap;
		}

		#region Implementation of ICriteriaEvent

		public void OnEvent(ICriteria crit)
		{
			subCriteriaRegistrar.EstablishSubCriteria(crit, subcriteriaFactory, shardToCriteriaMap, shardToCriteriaEventListMap);
		}

		#endregion
	}
}