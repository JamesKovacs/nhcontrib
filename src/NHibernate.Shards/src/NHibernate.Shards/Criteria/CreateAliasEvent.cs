using NHibernate.Shards.Session;
using NHibernate.SqlCommand;

namespace NHibernate.Shards.Criteria
{
	public class CreateAliasEvent : ICriteriaEvent
	{
		private enum MethodSig
		{
			AssocPathAndAlias,
			AssocPathAndAliasAndJoinType
		}

		private MethodSig methodSig;

		private string associationPath;

		private string alias;

		private JoinType joinType;

		private CreateAliasEvent(MethodSig methodSig, string associationPath, string alias, JoinType joinType)
		{
			this.methodSig = methodSig;
			this.associationPath = associationPath;
			this.alias = alias;
			this.joinType = joinType;
		}

		public CreateAliasEvent(string associationPath, string alias)
			: this(MethodSig.AssocPathAndAlias, associationPath, alias, JoinType.None)
		{
		}

		public CreateAliasEvent(string associationPath, string alias, JoinType joinType)
			: this(MethodSig.AssocPathAndAliasAndJoinType, associationPath, alias, joinType)
		{
		}

		#region Implementation of ICriteriaEvent

		public void OnEvent(ICriteria crit)
		{
			switch (methodSig)
			{
				case MethodSig.AssocPathAndAlias:
					crit.CreateAlias(associationPath, alias);
					break;
				case MethodSig.AssocPathAndAliasAndJoinType:
					crit.CreateAlias(associationPath, alias, joinType);
					break;
				default:
					throw new ShardedSessionException("Unknown ctor type in CreateAliasEvent: " + methodSig);
			}
		}

		#endregion
	}
}