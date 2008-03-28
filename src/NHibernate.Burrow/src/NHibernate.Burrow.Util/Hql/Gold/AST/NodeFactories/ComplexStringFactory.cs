namespace NHibernate.Burrow.Util.Hql.Gold.AST.NodeFactories
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// Examines childs and creates a LiteralNode or a BinaryArithmeticOperator, a BinaryArithmeticOperator is used 
	/// to represent multiple string/property concatenations.
	/// </summary>
	public class ComplexStringFactory : NodeFactory
	{
		public ComplexStringFactory(Reduction reduction) : base(reduction)
		{
		}

		/// <summary>
		/// Builds up nodes that corresponds to that reduction (reduction 1->N nodes).
		/// </summary>
		/// <returns></returns>
		public override IList<Node> BuildUpNodes()
		{
			if (Reduction.GetToken(0).Name == "(")
			{
				Reduction.Tokens.RemoveAt(0);
				Reduction.Tokens.RemoveAt(Reduction.Tokens.Count - 1);
			}

			// A ComplexString can be:
			// - a literal node (a constant string)
			// - a property
			// - a parameter (not done yet)
			// - a string concatenation (including all types before)
			if (Reduction.Tokens.Count == 1)
			{
				return GetNodesByToken(Reduction.GetToken(0));
			}

			// If the reduction has more than 1 node
			List<Node> nodes = new List<Node>();
			nodes.Add(new BinaryArithmeticOperator(Reduction));
			return nodes;
		}
	}
}