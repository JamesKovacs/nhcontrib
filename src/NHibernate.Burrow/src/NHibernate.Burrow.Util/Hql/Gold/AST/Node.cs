using System.Collections.Generic;

namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using GoldParser;

	/// <summary>
	/// Represents a node in the tree
	/// </summary>
	public abstract class Node
	{
		#region Ctor

		public Node(Reduction sectionToInterpret)
		{
			this.sectionToInterpret = sectionToInterpret;
		}

		#endregion

		#region Properties

		private readonly Reduction sectionToInterpret;

		/// <summary>
		/// Gets the internal section of GOLD Parser to interpret.
		/// </summary>
		/// <value>The section to interpret.</value>
		protected virtual Reduction SectionToInterpret
		{
			get { return sectionToInterpret; }
		}

		/// <summary>
		/// Gets the childs that are important nodes, overriding this property can modify the childs while still using 
		/// the NodeFactory to find the right nodes.
		/// </summary>
		/// <value>The useful child tokens.</value>
		protected virtual IEnumerable<Token> UsefulChildTokens
		{
			get
			{
				Token[] tokens = (Token[])SectionToInterpret.Tokens.ToArray(typeof(Token));
				return new List<Token>(tokens);
			}
		}

		private List<Node> childNodes = null;
		/// <summary>
		/// Gets the childs. This method may be called multiple times. Subsequent invocations are no-ops.
		/// </summary>
		/// <value>The childs.</value>
		public virtual IList<Node> ChildNodes
		{
			get
			{
				if (childNodes != null)
					return childNodes;

				childNodes = new List<Node>();
				foreach (Token token in UsefulChildTokens)
				{
					// Find the node related to the Reduction, if it's a Node add it directly,
					// if it's a Factory ask the factory for the items.
					childNodes.AddRange(NodeFactory.GetNodesByToken(token));
				}

				return childNodes;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns the first child node of type T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Get<T>() where T : class
		{
			foreach (Node node in ChildNodes)
			{
				T t = node as T;
				if (t != null)
					return t;
			}
			return null;
		}

		public override string ToString()
		{
			return GetType().Name;
		}

		#endregion
	}
}
