namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// Represents a negation or minus simbol
	/// </summary>
	public class NegateOperator : UnaryOperator
	{
		public NegateOperator(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
			if (sectionToInterpret.GetToken(0).Name == "-")
				sectionToInterpret.Tokens.RemoveAt(0);

			IList<Node> nodes = NodeFactory.GetNodesByToken(sectionToInterpret.GetToken(0));
			if (nodes.Count != 1)
				throw new AstException("Unable to parse the NegateOperator, only one direct child node is allowed.");
			
			AppliedTo = nodes[0];
		}
	}
}