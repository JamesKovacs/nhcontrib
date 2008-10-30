using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Linq.Expressions;
using NHibernate.Linq.Util;
using LinqExpression = System.Linq.Expressions.Expression;

namespace NHibernate.Linq.Visitors
{
	/// <summary>
	/// Translates a Linq Expression into an NHibernate ICriteria object.
	/// </summary>
	public class NHibernateQueryTranslator : NHibernateExpressionVisitor
	{
		private readonly ISession session;
		private ICriteria rootCriteria;
		private QueryOptions options;

		private object results;
		private bool hasResults;

		public NHibernateQueryTranslator(ISession session)
		{
			this.session = session;
		}

		private void ResetTranslator()
		{
			rootCriteria = null;
			results = null;
			hasResults = false;
		}

		public virtual object Translate(LinqExpression expression, QueryOptions queryOptions)
		{
			ResetTranslator();
			this.options = queryOptions;
			Visit(expression);

			if (hasResults)
				return results;

			return rootCriteria;
		}

		protected override LinqExpression VisitQuerySource(QuerySourceExpression expr)
		{
			if (rootCriteria == null)
			{
				rootCriteria = session.CreateCriteria(expr.ElementType, expr.Alias);
				options.Execute(rootCriteria);
			}
			return expr;
		}

		protected override LinqExpression VisitMethodCall(MethodCallExpression expr)
		{
			//create the root criteria if it is not already created
			Visit(expr.Arguments[0]);

			switch (expr.Method.Name)
			{
				case "Where":
					HandleWhereCall(expr);
					break;
				case "Select":
					HandleSelectCall(expr);
					break;
				case "OrderBy":
				case "ThenBy":
					HandleOrderByCall(expr);
					break;
				case "OrderByDescending":
				case "ThenByDescending":
					HandleOrderByDescendingCall(expr);
					break;
				case "Take":
					HandleTakeCall(expr);
					break;
				case "Skip":
					HandleSkipCall(expr);
					break;
				case "Distinct":
					HandleDistinctCall(expr);
					break;
				case "GroupBy":
					HandleGroupByCall(expr);
					break;
				case "SelectMany":
					HandleSelectManyCall(expr);
					break;
				case "OfType":
					//ignore OfType calls -- handled by InheritanceVisitor
					break;
				//case "GroupJoin":
				//    HandleGroupJoinCall(expr);
				//    break;
				case "First":
				case "FirstOrDefault":
				case "Single":
				case "SingleOrDefault":
				case "Aggregate":
				case "Average":
				case "Count":
				case "LongCount":
				case "Max":
				case "Min":
				case "Sum":
				case "Any":
					HandleImmediateResultsCall(expr);
					break;
				default:
					throw new NotImplementedException("The method " + expr.Method.Name + " is not implemented.");
			}

			return expr;
		}

		private void HandleWhereCall(MethodCallExpression call)
		{
			IEnumerable<ICriterion> criterion = WhereArgumentsVisitor.GetCriterion(rootCriteria, session, call.Arguments[1]);
			rootCriteria.Add(criterion);
		}

		private void HandleSelectCall(MethodCallExpression call)
		{
			var lambda = (LambdaExpression)LinqUtil.StripQuotes(call.Arguments[1]);

			var visitor = new SelectArgumentsVisitor(rootCriteria, session);
			visitor.Visit(lambda.Body);

			rootCriteria.SetProjectionIfNotNull(visitor.Projection);
			rootCriteria.SetResultTransformerIfNotNull(visitor.Transformer);
		}

		private void HandleOrderByCall(MethodCallExpression call)
		{
			LinqExpression expr = ((UnaryExpression)call.Arguments[1]).Operand;

			string name = MemberNameVisitor.GetMemberName(rootCriteria, expr);
			rootCriteria.AddOrder(Order.Asc(name));
		}

		private void HandleOrderByDescendingCall(MethodCallExpression call)
		{
			LinqExpression expr = ((UnaryExpression)call.Arguments[1]).Operand;

			string name = MemberNameVisitor.GetMemberName(rootCriteria, expr);
			rootCriteria.AddOrder(Order.Desc(name));
		}

		private void HandleTakeCall(MethodCallExpression call)
		{
			var count = (int)((ConstantExpression)call.Arguments[1]).Value;
			rootCriteria.SetMaxResults(count);
		}

		private void HandleSkipCall(MethodCallExpression call)
		{
			var index = (int)((ConstantExpression)call.Arguments[1]).Value;
			rootCriteria.SetFirstResult(index);
		}

		private void HandleDistinctCall(MethodCallExpression call)
		{
			var projection = rootCriteria.GetProjection() as PropertyProjection;
			if (projection != null)
			{
				rootCriteria.SetProjection(
					Projections.Distinct(
						Projections.Property(projection.PropertyName)));
			}
		}

		private void HandleGroupByCall(MethodCallExpression call)
		{
			var visitor = new GroupingArgumentsVisitor(rootCriteria);
			visitor.Visit(call.Arguments[1]);
			rootCriteria.SetProjectionIfNotNull(visitor.Projection);
		}

		private void HandleSelectManyCall(MethodCallExpression call)
		{
			//get the association path for the joined entity
			var collectionSelector = (LambdaExpression)LinqUtil.StripQuotes(call.Arguments[1]);

			LambdaExpression resultSelector = null;
			if (call.Arguments.Count == 3)
			{
				resultSelector = (LambdaExpression)LinqUtil.StripQuotes(call.Arguments[2]);
				string alias = resultSelector.Parameters[1].Name;

				var visitor = new SelectManyVisitor(rootCriteria, alias);
				visitor.Visit(collectionSelector.Body);
			}

			if (resultSelector != null)
			{
				//visit the result selector expression after the alias for the association has been created
				var resultSelectorVisitor = new SelectArgumentsVisitor(rootCriteria, session);
				resultSelectorVisitor.Visit(resultSelector.Body);

				rootCriteria.SetProjectionIfNotNull(resultSelectorVisitor.Projection);
				rootCriteria.SetResultTransformerIfNotNull(resultSelectorVisitor.Transformer);
			}
		}

		private void HandleGroupJoinCall(MethodCallExpression call)
		{
			//var leftQuery = call.Arguments[0] as ConstantExpression;
			//var rightQuery = call.Arguments[1] as ConstantExpression;
			//var leftKey = call.Arguments[2];
			//var rightKey = call.Arguments[3];
			//var lambda = call.Arguments[4] as LambdaExpression;
		}

		private void HandleImmediateResultsCall(MethodCallExpression call)
		{
			System.Type resultType = call.Method.ReturnType;
			System.Type visitorType = typeof(ImmediateResultsVisitor<>)
				.MakeGenericType(resultType);

			var visitor = (IImmediateResultsVisitor)Activator
				.CreateInstance(visitorType, session, rootCriteria);

			hasResults = true;
			results = visitor.GetResults(call);
		}
	}
}
