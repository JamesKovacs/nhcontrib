using System.Collections.Generic;

namespace NHibernate.Shards.Criteria
{
	public interface ISubcriteriaFactory
	{
		ICriteria CreateSubcriteria(ICriteria parent, IEnumerable<ICriteriaEvent> events);
	}
}