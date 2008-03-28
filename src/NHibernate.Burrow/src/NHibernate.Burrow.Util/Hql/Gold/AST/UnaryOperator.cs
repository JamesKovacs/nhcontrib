namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;

	public abstract class UnaryOperator : Node, IOperator
	{
		private Node appliedTo;

		protected UnaryOperator(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
		}

		public UnaryOperator(Node appliedTo)
			: base(null)
		{
			this.appliedTo = appliedTo;
		}

		/// <summary>
		/// Gets the inner node where the operator is applied.
		/// </summary>
		/// <value>The inner node.</value>
		public Node AppliedTo
		{
			get { return appliedTo; }
			protected set { appliedTo = value; }
		}

		public override IList<Node> ChildNodes
		{
			get
			{
				List<Node> nodes = new List<Node>(1);
				nodes.Add(appliedTo);
				return nodes;
			}
		}
	}
}