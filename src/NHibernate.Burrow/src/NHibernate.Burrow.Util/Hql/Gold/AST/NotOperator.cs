namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;

	public class NotOperator : UnaryOperator
	{
		public NotOperator(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
			if (sectionToInterpret.GetToken(0).Name == "not")
				sectionToInterpret.Tokens.RemoveAt(0);

			IList<Node> nodes = NodeFactory.GetNodesByToken(sectionToInterpret.GetToken(0));
			if (nodes.Count != 1)
				throw new AstException("Unable to parse the NotOperator, only one direct child node is allowed.");

			AppliedTo = nodes[0];
		}

		public NotOperator(Node appliedTo) : base(appliedTo)
		{
		}
	}
}