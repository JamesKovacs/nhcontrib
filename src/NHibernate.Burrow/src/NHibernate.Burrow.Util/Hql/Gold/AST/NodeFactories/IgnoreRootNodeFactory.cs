namespace NHibernate.Burrow.Util.Hql.Gold.AST.NodeFactories
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// Ignores the given node and returns all the child nodes.
	/// </summary>
	public class IgnoreRootNodeFactory : NodeFactory
	{
		public IgnoreRootNodeFactory(Reduction reduction) : base(reduction)
		{
		}

		public override IList<Node> BuildUpNodes()
		{
			List<Node> childNodes = new List<Node>();
			foreach (Token token in Reduction.Tokens)
			{
				// Find the node related to the Reduction, if it's a Node add it directly,
				// if it's a Factory ask the factory for the items.
				childNodes.AddRange(GetNodesByToken(token));
			}

			return childNodes;
		}
	}
}