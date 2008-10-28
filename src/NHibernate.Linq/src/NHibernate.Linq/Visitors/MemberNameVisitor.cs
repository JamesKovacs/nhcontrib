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
        private readonly ICriteria _rootCriteria;
        private readonly bool _createCriteriaForCollections;
        private ICriteria _currentCriteria;
        private Expression _currentExpression;
        private StringBuilder _memberNameBuilder;
    	private string _curretnAssociationPath;

        public string MemberName
        {
            get
            {
                if (_memberNameBuilder.Length < 1)
					return _curretnAssociationPath;
                string memberName = _memberNameBuilder.ToString();
                return memberName.Substring(0, memberName.Length - 1); //remove the last "."
            }
        }

        public ICriteria CurrentCriteria
        {
            get { return _currentCriteria; }
        }

        public Expression CurrentExpression
        {
            get { return _currentExpression; }
        }

        public MemberNameVisitor(ICriteria criteria)
            : this(criteria, false) { }

        public MemberNameVisitor(ICriteria criteria, bool createCriteriaForCollections)
        {
            _rootCriteria = _currentCriteria = criteria;
            _createCriteriaForCollections = createCriteriaForCollections;
            _memberNameBuilder = new StringBuilder();
        }

        private void ResetMemberName(string name)
        {
            _memberNameBuilder = new StringBuilder();
            _memberNameBuilder.Append(name);
        }

        private ICriteria EnsureCriteria(string associationPath, string alias)
        {
            ICriteria criteria;
            if ((criteria = _currentCriteria.GetCriteriaByAlias(alias)) == null)
            {
                criteria = _currentCriteria.CreateCriteria(associationPath, alias, JoinType.LeftOuterJoin);
            }
            return criteria;
        }

		protected override Expression VisitEntity(EntityExpression expr)
		{
			expr = (EntityExpression)base.VisitEntity(expr);

			if (expr.Expression != null || expr.Type != _rootCriteria.GetRootType())
			{
				if (!String.IsNullOrEmpty(expr.AssociationPath))
					_currentCriteria = EnsureCriteria(expr.AssociationPath, expr.Alias);

				ResetMemberName(expr.Alias + ".");
			}
			_curretnAssociationPath = expr.AssociationPath;
			_currentExpression = expr;
			return expr;
		}

        protected override Expression VisitPropertyAccess(PropertyAccessExpression expr)
        {
            expr = (PropertyAccessExpression)base.VisitPropertyAccess(expr);
            _memberNameBuilder.Append(expr.Name + ".");

            _currentExpression = expr;
            return expr;
        }

        protected override Expression VisitCollectionAccess(CollectionAccessExpression expr)
        {
            expr = (CollectionAccessExpression)base.VisitCollectionAccess(expr);
            _memberNameBuilder.Append(expr.Name + ".");

            _currentExpression = expr;

            if (_createCriteriaForCollections)
            {
                if (expr.ElementExpression != null)
                {
                    EnsureCriteria(expr.Name, expr.ElementExpression.Alias);
                }
            }

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
