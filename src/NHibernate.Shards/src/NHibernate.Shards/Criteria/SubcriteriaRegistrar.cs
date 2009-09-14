using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Shards.Criteria
{
    class SubcriteriaRegistrar:ISubcriteriaRegistrar
    {
        private readonly IShard shard;

        public SubcriteriaRegistrar(IShard shard)
        {
            this.shard = shard;
        }

        public void EstablishSubCriteria(ICriteria parentCriteria, ISubcriteriaFactory subcriteriaFactory, IDictionary<IShard, ICriteria> shardToCriteriaMap, IDictionary<IShard, IList<ICriteriaEvent>> shardToCriteriaEventListMap)
        {
            IList<ICriteriaEvent> criteriaEvents = shardToCriteriaEventListMap[shard];
            ICriteria newCrit = subcriteriaFactory.CreateSubcriteria(parentCriteria, criteriaEvents);
            criteriaEvents.Clear();
            shardToCriteriaMap[shard] = newCrit;
        }
    }
}
