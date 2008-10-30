using System;
using System.Linq.Expressions;
using System.Text;
using NHibernate.Linq.Util;
using NHibernate.Metadata;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Linq.Expressions;

namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// Visits an expression providing the member name being accessed based on the EntityExpressions and
    /// PropertyAccessExpressions in the expression tree. Any entity associations encountered are added
    /// as subcriteria to the query.
    /// </summary>
    public class MemberNameVisitor : NHibernateExpressionVisitor
    {
        private readonly ICriteria rootCriteria;
        private readonly bool createCriteriaForCollections;
        private ICriteria currentCriteria;
        private Expression currentExpression;
        private StringBuilder memberNameBuilder;
    	private string currentAssociationPath;
		private bool isQueringEntity;

        public string MemberName
        {
            get
            {
				if (isQueringEntity)
					return currentAssociationPath;
                string memberName = memberNameBuilder.ToString();
                return memberName.Substring(0, memberName.Length - 1); //remove the last "."
            }
        }

        public ICriteria CurrentCriteria
        {
            get { return currentCriteria; }
        }

        public Expression CurrentExpression
        {
            get { return currentExpression; }
        }

        public MemberNameVisitor(ICriteria criteria)
            : this(criteria, false) { }

        public MemberNameVisitor(ICriteria criteria, bool createCriteriaForCollections)
        {
            this.rootCriteria = this.currentCriteria = criteria;
            this.createCriteriaForCollections = createCriteriaForCollections;
            this.memberNameBuilder = new StringBuilder();
        }

        private void ResetMemberName(string name)
        {
            memberNameBuilder = new StringBuilder();
            memberNameBuilder.Append(name);
        }

        private ICriteria EnsureCriteria(string associationPath, string alias)
        {
            ICriteria criteria;
            if ((criteria = currentCriteria.GetCriteriaByAlias(alias)) == null)
            {
                criteria = currentCriteria.CreateCriteria(associationPath, alias, JoinType.LeftOuterJoin);
            }
            return criteria;
        }

		protected override Expression VisitEntity(EntityExpression expr)
		{
			expr = (EntityExpression)base.VisitEntity(expr);

			if (expr.Expression != null || expr.Type != rootCriteria.GetRootType())
			{
				if (!String.IsNullOrEmpty(expr.AssociationPath))
					currentCriteria = EnsureCriteria(expr.AssociationPath, expr.Alias);

				ResetMemberName(expr.Alias + ".");
			}
			currentAssociationPath = expr.AssociationPath;
			currentExpression = expr;
			isQueringEntity = true;

			return expr;
		}

        protected override Expression VisitPropertyAccess(PropertyAccessExpression expr)
        {
            expr = (PropertyAccessExpression)base.VisitPropertyAccess(expr);
            memberNameBuilder.Append(expr.Name + ".");

            currentExpression = expr;
			isQueringEntity = false;

            return expr;
        }

        protected override Expression VisitCollectionAccess(CollectionAccessExpression expr)
        {
            expr = (CollectionAccessExpression)base.VisitCollectionAccess(expr);
            memberNameBuilder.Append(expr.Name + ".");

            currentExpression = expr;

            if (createCriteriaForCollections)
            {
                if (expr.ElementExpression != null)
                {
                    EnsureCriteria(expr.Name, expr.ElementExpression.Alias);
                }
            }

			isQueringEntity = false;

            return expr;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expr)
        {
            Visit(expr.Arguments[0]);
            return expr;
        }

        public static string GetMemberName(ICriteria rootCriteria, Expression expr)
        {
            MemberNameVisitor visitor = new MemberNameVisitor(rootCriteria);
            visitor.Visit(expr);
            return visitor.MemberName;
        }
    }
}
