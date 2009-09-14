namespace NHibernate.Shards.Criteria
{
	public interface IShardedSubcriteria : ICriteria
	{
		IShardedCriteria ParentCriteria { get; }
	}
}