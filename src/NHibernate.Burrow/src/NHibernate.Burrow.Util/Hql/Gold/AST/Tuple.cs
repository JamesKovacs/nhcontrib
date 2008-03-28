namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// The format how the tuple is written
	/// </summary>
	public enum TupleFormat
	{
		/// <summary>
		/// The tuple is written as a list of values separated by commas
		/// </summary>
		CommaSeparated,
		/// <summary>
		/// The tuple is written as a subquery
		/// </summary>
		SubQuery
	}

	/// <summary>
	/// Represents a list of values, might be separated by comma or expresated in a subquery
	/// </summary>
	public class Tuple : Node
	{
		public Tuple(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
		}

		/// <summary>
		/// Gets the tuple format.
		/// </summary>
		/// <value>The format.</value>
		public TupleFormat Format
		{
			get
			{
				if (Get<SelectStatement>() != null)
					return TupleFormat.SubQuery;
				return TupleFormat.CommaSeparated;
			}
		}

		private List<Node> childNodes = null;
		/// <summary>
		/// Gets the childs. This method may be called multiple times. Subsequent invocations are no-ops.
		/// </summary>
		/// <value>The childs.</value>
		public override IList<Node> ChildNodes
		{
			get
			{
				if (childNodes != null)
					return childNodes;

				// If the reduction is a part of a query, return a SelectStatement
				Token tupleContent = SectionToInterpret.GetToken(1);
				Reduction reduction = tupleContent.Data as Reduction;

				if (reduction == null)
				{
					// If the tuple contains only a simple value
					return base.ChildNodes;
				}

				string reductionName = reduction.ParentRule.RuleNonTerminal.Name;
				if (reductionName == "QueryLogic" || reductionName == "FromClause")
				{
					// ie: from Entity [where ...]
					childNodes = new List<Node>();
					SelectStatement statement = new SelectStatement(NodeFactory.GetNodesByToken(tupleContent));
					childNodes.Add(statement);
					return childNodes;
				}
				else if (reductionName == "SelectQuery")
				{
					// ie: select ... from  ...
					childNodes = new List<Node>();
					childNodes.AddRange(NodeFactory.GetNodesByToken(tupleContent));
					return childNodes;
				}
				else
				{
					// ie: (1, 2, 4) or ("John", "Peter")
					// If it's defined as a comma separated value list, use the token stream to build the childs
					return base.ChildNodes;
				}
			}
		}

		protected override IEnumerable<Token> UsefulChildTokens
		{
			get
			{
				// It's called ONLY when the tuple is defined as comma separated values
				// Return all items in the tuple, removing the parentheses and separators
				foreach (Token token in ReductionsHelper.FlatternChilds(SectionToInterpret, "ConstantList"))
				{
					if (token.Name != "(" &&
						token.Name != ")" &&
						token.Name != ",")
					{
						yield return token;
					}
				}
			}
		}
	}
}