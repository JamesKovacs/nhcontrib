using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using NHibernate.Linq.Util;
using NHibernate.Metadata;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Linq.Expressions;
using NHibernate.Engine;

namespace NHibernate.Linq.Visitors
{
    /// <summary>
    /// Preprocesses an expression tree replacing MemberAccessExpressions and ParameterExpressions with
    /// NHibernate-specific PropertyAccessExpressions and EntityExpressions respectively.
    /// </summary>
    public class AssociationVisitor : ExpressionVisitor
    {
        private readonly ISessionFactory _sessionFactory;

        public AssociationVisitor(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public static Expression RewriteWithAssociations(ISessionFactory sessionFactory, Expression expression)
        {
            AssociationVisitor visitor = new AssociationVisitor(sessionFactory);
            return visitor.Visit(expression);
        }

        private IClassMetadata GetMetaData(System.Type type)
        {
            if (!LinqUtil.IsAnonymousType(type))
            {
                try
                {
                    return _sessionFactory.GetClassMetadata(type);
                }
                catch (MappingException) { /* ignore non-persisted classes */ }
            }
            return null;
        }

        private EntityExpression GetParentExpression(MemberExpression expr, out string memberName, out IType nhibernateType)
        {
			memberName = null;
			nhibernateType = null;

			CollectionAccessExpression collectionExpr = expr.Expression as CollectionAccessExpression;
			if (collectionExpr != null)
			{
				return null;
			}

            PropertyAccessExpression propExpr = expr.Expression as PropertyAccessExpression;
            if (propExpr != null)
            {
                memberName = propExpr.Name + "." + expr.Member.Name;
                nhibernateType = propExpr.Expression.MetaData.GetPropertyType(memberName);
                return propExpr.Expression;
            }

            EntityExpression entityExpr = expr.Expression as EntityExpression;
            if (entityExpr != null)
            {
                memberName = expr.Member.Name;
                nhibernateType = entityExpr.MetaData.GetPropertyType(memberName);
                return entityExpr;
            }

            return null;
        }

        protected override Expression VisitMemberAccess(MemberExpression expr)
        {
            expr = (MemberExpression)base.VisitMemberAccess(expr);

            IClassMetadata metaData = GetMetaData(expr.Type);
            if (metaData != null)
                return new EntityExpression(expr.Member.Name, expr.Member.Name, expr.Type, metaData, expr.Expression);

            string memberName;
            IType nhibernateType;
            EntityExpression parentExpression = GetParentExpression(expr, out memberName, out nhibernateType);

            if (parentExpression != null)
            {
                if (nhibernateType.IsCollectionType)
                {
					CollectionType collectionType = (CollectionType)nhibernateType;
					IType nhElementType = collectionType.GetElementType((ISessionFactoryImplementor)_sessionFactory);

					System.Type elementType = nhElementType.ReturnedClass;
                    IClassMetadata elementMetaData = GetMetaData(elementType);

                    EntityExpression elementExpression = null;
                    if (elementMetaData != null)
                        elementExpression = new EntityExpression(null, memberName, elementType, elementMetaData, null);

                    return new CollectionAccessExpression(memberName, expr.Type, nhibernateType, parentExpression, elementExpression);
                }

                return new PropertyAccessExpression(memberName, expr.Type, nhibernateType, parentExpression);
            }

            return expr;
        }

        protected override Expression VisitParameter(ParameterExpression expr)
        {
            IClassMetadata metaData = GetMetaData(expr.Type);
            if (metaData != null)
                return new EntityExpression(null, expr.Name, expr.Type, metaData, null);

            return expr;
        }

        protected override Expression VisitConstant(ConstantExpression expr)
        {
            IQueryable query = expr.Value as IQueryable;
            if (query != null)
            {
                return new QuerySourceExpression("this", query);
            }
            return expr;
        }
    }
}
