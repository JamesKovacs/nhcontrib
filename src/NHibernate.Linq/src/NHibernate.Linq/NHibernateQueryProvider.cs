using System;
using System.Linq.Expressions;
using NHibernate.Engine;
using NHibernate.Linq.Util;
using NHibernate.Linq.Visitors;

namespace NHibernate.Linq
{
	public class NHibernateQueryProvider : QueryProvider
	{
		private readonly ISession session;
		private readonly string entityName;
		private readonly ICriteria rootCriteria;

		public NHibernateQueryProvider(ISession session, QueryOptions queryOptions)
			: this(session, queryOptions, null) { }

		public NHibernateQueryProvider(ISession session, QueryOptions queryOptions, string entityName)
		{
			if (session == null) throw new ArgumentNullException("session");

			this.session = session;
			this.entityName = entityName;
			this.queryOptions = queryOptions;
		}

		public NHibernateQueryProvider(ISession session, ICriteria rootCriteria)
		{
			if (session == null) throw new ArgumentNullException("session");
			if (rootCriteria == null) throw new ArgumentNullException("rootCriteria");

			this.session = session;
			this.rootCriteria = rootCriteria;
		}

		private static object ResultsFromCriteria(ICriteria criteria, Expression expression)
		{
			System.Type elementType = TypeSystem.GetElementType(expression.Type);

			return Activator.CreateInstance(typeof(CriteriaResultReader<>)
			  .MakeGenericType(elementType), criteria);
		}

		public object TranslateExpression(Expression expression)
		{
			expression = Evaluator.PartialEval(expression);
			expression = new BinaryBooleanReducer().Visit(expression);
			expression = new AssociationVisitor((ISessionFactoryImplementor)session.SessionFactory).Visit(expression);
			expression = new InheritanceVisitor().Visit(expression);
			expression = CollectionAliasVisitor.AssignCollectionAccessAliases(expression);
			expression = new PropertyToMethodVisitor().Visit(expression);
			expression = new BinaryExpressionOrderer().Visit(expression);

			var translator = new NHibernateQueryTranslator(session, entityName);

			return this.rootCriteria == null ?
				translator.Translate(expression, this.queryOptions) :
				translator.Translate(expression, this.rootCriteria);
		}

		public override object Execute(Expression expression)
		{
			var results = TranslateExpression(expression);
			var criteria = results as ICriteria;

			if (criteria != null)
				return ResultsFromCriteria(criteria, expression);
			return results;
		}
	}
}