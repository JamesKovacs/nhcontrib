using NHibernate.Shards.Session;

namespace NHibernate.Shards.Criteria
{
	public class CriteriaFactoryImpl : ICriteriaFactory
	{
		private enum MethodSig
		{
			Class,
			ClassAndAlias,
			Entity,
			EntityAndAlias
		}

		private MethodSig methodSig;
		private System.Type persistentClass;
		private string alias;
		private string entityName;

		private CriteriaFactoryImpl(MethodSig methodSig, System.Type persistentClass, string alias, string entityName)
		{
			this.methodSig = methodSig;
			this.persistentClass = persistentClass;
			this.alias = alias;
			this.entityName = entityName;
		}

		public CriteriaFactoryImpl(System.Type persistentClass) : this(MethodSig.Class, persistentClass, null, null)
		{
		}

		public CriteriaFactoryImpl(System.Type persistentClass, string alias)
			: this(MethodSig.ClassAndAlias, persistentClass, alias, null)
		{
		}

		public CriteriaFactoryImpl(string entityName) : this(MethodSig.Entity, null, null, entityName)
		{
		}

		public CriteriaFactoryImpl(string entityName, string alias) : this(MethodSig.EntityAndAlias, null, alias, entityName)
		{
		}

		public ICriteria CreateCriteria(ISession session)
		{
			switch (methodSig)
			{
				case MethodSig.Class:
					return session.CreateCriteria(persistentClass);
				case MethodSig.ClassAndAlias:
					return session.CreateCriteria(persistentClass, alias);
				case MethodSig.Entity:
					return session.CreateCriteria(entityName);
				case MethodSig.EntityAndAlias:
					return session.CreateCriteria(entityName, alias);
				default:
					throw new ShardedSessionException("Unknown constructor type for criteria create: " + methodSig);
			}
		}
	}
}