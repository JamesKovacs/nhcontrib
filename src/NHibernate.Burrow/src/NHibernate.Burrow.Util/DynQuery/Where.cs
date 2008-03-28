using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Burrow.Util.DynQuery
{
	[Serializable]
	public class Where : IDynClause
	{
		private readonly bool negatingAll;
		private List<IQueryPart> expressions = new List<IQueryPart>();

		public Where() { }

		public Where(bool negatingAll)
		{
			this.negatingAll = negatingAll;
		}

		public Where(string expression)
			: this(false)
		{
			expressions.Add(new LogicalExpression(LogicalOperator.Null, expression));
		}

		public Where And(string expression)
		{
			expressions.Add(new LogicalExpression((HasMembers) ? LogicalOperator.And : LogicalOperator.Null, expression));
			return this;
		}

		public Where Or(string expression)
		{
			expressions.Add(new LogicalExpression((HasMembers) ? LogicalOperator.Or : LogicalOperator.Null, expression));
			return this;			
		}

		public Where Not(string expression)
		{
			expressions.Add(new LogicalExpression(LogicalOperator.Not, expression));
			return this;
		}

		public Where Clone()
		{
			Where result = new Where(negatingAll);
			result.expressions = null;
			result.expressions = new List<IQueryPart>(expressions);
			return result;
		}

		#region IDynClause Members

		/// <summary>
		/// The query clause.
		/// </summary>
		public string Clause
		{
			get
			{
				return (!HasMembers) ? string.Empty : 
					string.Format("{0} ({1})", (negatingAll) ? "where not" : "where", Expression);
			}
		}

		#region IQueryPart Members

		/// <summary>
		/// The query part.
		/// </summary>
		public string Expression
		{
			get { return GetExpression(); }
		}

		#endregion

		/// <summary>
		/// The clause has some meber or not?
		/// </summary>
		public bool HasMembers
		{
			get { return expressions.Count > 0; }
		}

		#endregion

		private string GetExpression()
		{
			if (!HasMembers) return string.Empty;
			StringBuilder clause = new StringBuilder();

			IEnumerator<IQueryPart> iter = expressions.GetEnumerator();
			iter.MoveNext();
			clause.Append(iter.Current.Expression);
			while (iter.MoveNext())
				clause.Append(iter.Current.Clause);
			return clause.ToString();
		}
	}
}