namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;
	using NHibernate;

	/// <summary>
	/// Represents the from clause inside a select statement
	/// </summary>
	public class FromClause : Node
	{
		public FromClause(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
			// Remove the "from" text
			sectionToInterpret.Tokens.RemoveAt(0);
		}

		/// <summary>
		/// Gets the childs that are important nodes, overriding this property can modify the childs while still using
		/// the NodeFactory to find the right nodes.
		/// </summary>
		/// <value>The useful child tokens.</value>
		protected override IEnumerable<Token> UsefulChildTokens
		{
			get
			{
				return ReductionsHelper.FlatternChilds(SectionToInterpret, "EntityList");
			}
		}

		/// <summary>
		/// Gets the childs. This method may be called multiple times. Subsequent invocations are no-ops.
		/// </summary>
		/// <value>The childs.</value>
		public override IList<Node> ChildNodes
		{
			get
			{
				List<Node> childNodes = new List<Node>();
				foreach (Node node in base.ChildNodes)
				{
					if (node is Identifier || node is Property)
						childNodes.Add(new FromElement(node));
					else
						childNodes.Add(node);
				}
				return childNodes;
			}
		}
	}
}