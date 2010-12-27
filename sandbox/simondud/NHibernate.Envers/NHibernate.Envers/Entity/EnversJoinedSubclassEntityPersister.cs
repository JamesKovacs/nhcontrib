using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Persister.Entity;

namespace NHibernate.Envers.Entity
{
	public class EnversJoinedSubclassEntityPersister : JoinedSubclassEntityPersister
	{
		public EnversJoinedSubclassEntityPersister(PersistentClass persistentClass, ICacheConcurrencyStrategy cache, ISessionFactoryImplementor factory, IMapping mapping) 
												: base(persistentClass, cache, factory, mapping)
		{
		}
	}
}