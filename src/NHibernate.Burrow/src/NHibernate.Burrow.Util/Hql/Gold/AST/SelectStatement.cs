namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;
	using NHibernate;

	/// <summary>
	/// Represents a hql select statement, with all included
	/// </summary>
	public class SelectStatement : Node, IStatement
	{
		#region Ctor

		public SelectStatement(Reduction reduction)
			: base(reduction)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SelectStatement"/> class with the given child nodes or node factory.
		/// </summary>
		/// <param name="childsInformation">The childs information.</param>
		public SelectStatement(object childsInformation)
			: base(null)
		{
			Node node = childsInformation as Node;
			NodeFactory factory = childsInformation as NodeFactory;

			if (node != null)
			{
				childNodes = new List<Node>(1);
				childNodes.Add(node);
			}
			else if (factory != null)
			{
				childNodes = new List<Node>();
				((List<Node>)childNodes).AddRange(factory.BuildUpNodes());
			}
			else
			{
				throw new TypeMismatchException("The given childs information could not be parsed.");
			}

			overridenInfo = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SelectStatement"/> class with the given child nodes
		/// </summary>
		/// <param name="childNodes">The child nodes.</param>
		public SelectStatement(IList<Node> childNodes)
			: base(null)
		{
			this.childNodes = childNodes;
			overridenInfo = true;
		}

		#endregion

		#region Properties

		private readonly bool overridenInfo = false;

		private readonly IList<Node> childNodes;

		public override IList<Node> ChildNodes
		{
			get
			{
				// If the information was manually set return the nodes
				if (overridenInfo)
				{
					return childNodes;
				}
				return base.ChildNodes;
			}
		}

		#endregion
	}
}