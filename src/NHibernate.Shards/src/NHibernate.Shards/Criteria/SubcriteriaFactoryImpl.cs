using System.Collections.Generic;
using NHibernate.Shards.Session;
using NHibernate.SqlCommand;

namespace NHibernate.Shards.Criteria
{
    public class SubcriteriaFactoryImpl:ISubcriteriaFactory
    {
        private enum MethodSig
        {
            Association,
            AssociationAndJoinType,
            AssociationAndAlias,
            AssociationAndAliasAndJoinType
        }

        private readonly MethodSig methodSig;

        private readonly JoinType joinType;

        private readonly string association;

        private readonly string alias;

        private SubcriteriaFactoryImpl(MethodSig methodSig, string association, JoinType joinType, string alias)
        {
            this.methodSig = methodSig;
            this.association = association;
            this.joinType = joinType;
            this.alias = alias;
        }

        public SubcriteriaFactoryImpl(string association):this(MethodSig.Association,association,JoinType.None,null)
        {            
        }

        public SubcriteriaFactoryImpl(string association, JoinType joinType)
            : this(MethodSig.AssociationAndJoinType, association, joinType, null)
        {
            
        }

        public SubcriteriaFactoryImpl(string association,string alias):this(MethodSig.AssociationAndAlias,association,JoinType.None,alias)
        {
            
        }

        public SubcriteriaFactoryImpl(string association, string alias, JoinType joinType):this(MethodSig.AssociationAndAliasAndJoinType,association,joinType,alias)
        {
            
        }

        public ICriteria CreateSubcriteria(ICriteria parent, IEnumerable<ICriteriaEvent> criteriaEvents)
        {
            ICriteria crit;
            switch(methodSig)
            {
                case MethodSig.Association:
                    crit = parent.CreateCriteria(association);
                    break;
                case MethodSig.AssociationAndJoinType:
                    crit = parent.CreateCriteria(association, joinType);
                    break;
                case MethodSig.AssociationAndAlias:
                    crit = parent.CreateCriteria(association, alias);
                    break;
                case MethodSig.AssociationAndAliasAndJoinType:
                    crit = parent.CreateCriteria(association, alias, joinType);
                    break;
                default:
                    throw new ShardedSessionException("Unknown constructor type for subcriteria creation: " + methodSig);
            }
            foreach(ICriteriaEvent criteriaEvent in criteriaEvents)
            {
                criteriaEvent.OnEvent(crit);
            }
            return crit;
        }

    }
}
