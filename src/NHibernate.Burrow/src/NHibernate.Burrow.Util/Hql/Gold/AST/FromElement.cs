namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// Represents a element in the from clause
	/// </summary>
	public class FromElement : Node
	{
		#region Ctors

		public FromElement(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
			// The tokens can be in the following formats:
			// <Property> | <Property> as Identifier | Identifier in class <Property>

			// I will change the order of the tokens to have: <Object> <Alias>

			if (sectionToInterpret.Tokens.Count == 3)
			{
				sectionToInterpret.Tokens.RemoveAt(1);
			}
			else if (sectionToInterpret.Tokens.Count == 4)
			{
				sectionToInterpret.Tokens.RemoveAt(1);
				sectionToInterpret.Tokens.RemoveAt(1);
				sectionToInterpret.Tokens.Reverse();
			}
		}

		public FromElement(Node childNode) : base(null)
		{
			this.childNode = childNode;
		}

		#endregion

		#region Properties

		private readonly Node childNode;

		protected override IEnumerable<Token> UsefulChildTokens
		{
			get
			{
				if (childNode == null)
				{
					foreach (Token token in base.UsefulChildTokens)
					{
						yield return token;
					}
				}
			}
		}

		public override IList<Node> ChildNodes
		{
			get { return new List<Node>(); }
		}

		/// <summary>
		/// Gets the name of the class.
		/// </summary>
		/// <value>The name of the class.</value>
		public string ClassName
		{
			get
			{
				// if the child was overriden, use it
				Node node = childNode ?? base.ChildNodes[0];
				Identifier identifier = node as Identifier;
				if (identifier != null)
				{
					return identifier.Name;
				}
				else
				{
					return ((Property) node).Name;
				}
			}
		}

		/// <summary>
		/// Gets the entity alias.
		/// </summary>
		/// <value>The alias.</value>
		public string Alias
		{
			get
			{
				if (childNode == null)
				{
					if (base.ChildNodes.Count == 2)
					{
						return ((Identifier)base.ChildNodes[1]).Name;
					}
				}
				return null;
			}
		}

		#endregion

		public override string ToString()
		{
			string aliasText = Alias == null ? "" : " as " + Alias;
			return string.Format("{0}: {1}{2}", base.ToString(), ClassName, aliasText);
		}
	}
}