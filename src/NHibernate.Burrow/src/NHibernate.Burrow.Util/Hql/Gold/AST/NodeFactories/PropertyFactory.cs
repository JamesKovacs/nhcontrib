namespace NHibernate.Burrow.Util.Hql.Gold.AST.NodeFactories
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// Examines all childs and creates a Property or Identifier.
	/// </summary>
	public class PropertyFactory : NodeFactory
	{
		public PropertyFactory(Reduction reduction) : base(reduction)
		{
		}

		/// <summary>
		/// Builds up nodes that corresponds to that reduction (reduction 1-&gt;N nodes).
		/// </summary>
		/// <returns></returns>
		public override IList<Node> BuildUpNodes()
		{
			ReductionsHelper.MergeChilds(Reduction, "");

			List<Node> nodes = new List<Node>();
			if (Reduction.Tokens.Count > 0)
			{
				Token token = (Token) Reduction.Tokens[0];
				string text = token.Data.ToString();

				if (text.Contains("."))
					nodes.Add(new Property(text));
				else
					nodes.Add(new Identifier(text));
			}
			return nodes;
		}
	}
}