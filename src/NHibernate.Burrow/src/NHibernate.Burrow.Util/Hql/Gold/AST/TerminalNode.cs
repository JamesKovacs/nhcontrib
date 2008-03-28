namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// Represents a node that has no chlids
	/// </summary>
	public abstract class TerminalNode : Node
	{
		protected TerminalNode(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
		}

		/// <summary>
		/// The terminal nodes have no childs.
		/// </summary>
		/// <value>The childs.</value>
		public override IList<Node> ChildNodes
		{
			get { return new List<Node>(); }
		}
	}
}