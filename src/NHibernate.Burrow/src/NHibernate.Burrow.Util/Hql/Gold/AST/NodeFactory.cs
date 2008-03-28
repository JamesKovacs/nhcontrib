using System;
using System.Collections.Generic;

namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Reflection;
	using GoldParser;
	using NHibernate;
	using NodeFactories;
    using Type = System.Type;

	/// <summary>
	/// Creates the nodes according to the node name
	/// </summary>
	public abstract class NodeFactory
	{
		#region Lookup for Nodes or NodeFactories

		private static readonly Dictionary<string, Type> nodeManagers = new Dictionary<string, Type>();
		private static readonly List<string> ignoredTokens = new List<string>();

		static NodeFactory()
		{
			LoadNodeManagers();

			ValidateNodeManagers();
		}

		/// <summary>Validates all nodes or node factory types are correct.</summary>
		private static void ValidateNodeManagers()
		{
			Dictionary<string, Type>.Enumerator etor = nodeManagers.GetEnumerator();
			while (etor.MoveNext())
			{
				Type type = etor.Current.Value;
				if (typeof(Node).IsAssignableFrom(type) ||
					typeof(NodeFactory).IsAssignableFrom(type))
				{
					ConstructorInfo nonTerminalCtor = type.GetConstructor(new Type[] {typeof (Reduction)});
					ConstructorInfo terminalCtor = type.GetConstructor(new Type[] { typeof(string) });
					if (nonTerminalCtor == null && terminalCtor == null)
						throw new TypeMismatchException("All Nodes and NodeFactories must have a constructor with one parameter of type Reduction or String");
				}
				else
				{
					throw new TypeMismatchException(string.Format(
						"The type '{0}' is not valid as a node manager, only Node and NodeFactory are allowed", type.FullName));
				}
			}
		}

		/// <summary>Loads the node managers using DI</summary>
		private static void LoadNodeManagers()
		{
			// TODO: Use a better dependency injection method

			nodeManagers.Add("SelectQuery", typeof(SelectStatement));
			nodeManagers.Add("DeleteQuery", typeof(DeleteStatement));
			nodeManagers.Add("SelectClause", typeof(SelectClause));
			nodeManagers.Add("QueryLogic", typeof(IgnoreRootNodeFactory));
			nodeManagers.Add("FromClause", typeof(FromClause));
			nodeManagers.Add("WhereClause", typeof(WhereClause));
			nodeManagers.Add("EntityDeclaration", typeof(FromElement));
			nodeManagers.Add("Identifier", typeof(Identifier));
			nodeManagers.Add("Property", typeof(PropertyFactory));

			nodeManagers.Add("Predicate", typeof(PredicateFactory));
			nodeManagers.Add("ExpAnd", typeof(BinaryLogicOperator));
			nodeManagers.Add("ExpOr", typeof(BinaryLogicOperator));
			nodeManagers.Add("ExpAdd", typeof(BinaryArithmeticOperator));
			nodeManagers.Add("ExpMultiply", typeof(BinaryArithmeticOperator));
			nodeManagers.Add("ExpNegate", typeof(NegateOperator));
			nodeManagers.Add("ExpNot", typeof(NotOperator));
			nodeManagers.Add("StringValue", typeof(BinaryArithmeticOperator));

			nodeManagers.Add("Value", typeof(IgnoreRootNodeFactory));
			nodeManagers.Add("Tuple", typeof(Tuple));
			nodeManagers.Add("StringLiteral", typeof(LiteralNode));
			nodeManagers.Add("IntValue", typeof(IntValue));
			nodeManagers.Add("ComplexString", typeof(ComplexStringFactory));
			nodeManagers.Add("Parameter", typeof(Parameter));


			// Parenthesis are ignore in grouped expressions, because the tree implicitly includes them. 
			ignoredTokens.Add("(");		
			ignoredTokens.Add(")");
			ignoredTokens.Add("+");
		}

		/// <summary>
		/// Gets all the managers by token (it can be a simple Node or a NodeFactory)
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public static IList<Node> GetNodesByToken(Token token)
		{
			string name;

			// If the token contains a reduction, is better to read the name from the reduction and not from the token itself
			Reduction reduction = token.Data as Reduction;
			if (reduction != null)
			{
				name = reduction.ParentRule.RuleNonTerminal.Name;
			}
			else
			{
				name = token.Name;
			}

			List<Node> childNodes = new List<Node>();

			if (ignoredTokens.Contains(name))
			{
				return childNodes;
			}

			if (!nodeManagers.ContainsKey(name))
			{
				throw new NotSupportedException(string.Format("The token found '{0}' is not supported.", name));
			}

			// Create the instance of the manager (can be Node or NodeFactory)
			Type managerType = nodeManagers[name];
			object managerInstance = null;
			if (token.Kind == SymbolType.NonTerminal)
			{
				ConstructorInfo nodeConstructor = managerType.GetConstructor(new Type[] { typeof(Reduction) });
				managerInstance = nodeConstructor.Invoke(new object[] { token.Data as Reduction });
			}
			else if (token.Kind == SymbolType.Terminal)
			{
				ConstructorInfo nodeConstructor = managerType.GetConstructor(new Type[] { typeof(string) });
				managerInstance = nodeConstructor.Invoke(new object[] { token.Data as string });
			}

			// Add the child managers
			Node node = managerInstance as Node;
			NodeFactory factory = managerInstance as NodeFactory;
			if (node != null)
			{
				childNodes.Add(node);
			}
			else if (factory != null)
			{
				childNodes.AddRange(factory.BuildUpNodes());
			}

			return childNodes;
		}

		/// <summary>
		/// Gets the manager by reduction (it can be a simple Node or a NodeFactory)
		/// </summary>
		/// <param name="reduction">The reduction.</param>
		/// <returns></returns>
		public static object GetManagerByReduction(Reduction reduction)
		{
			string name = reduction.ParentRule.RuleNonTerminal.Name;


			if (ignoredTokens.Contains(name))
			{
				return null;
			}

			if (!nodeManagers.ContainsKey(name))
			{
				throw new NotSupportedException(string.Format("The reduction found '{0}' is not supported.", name));
			}

			Type manager = nodeManagers[name];
			ConstructorInfo nodeConstructor = manager.GetConstructor(new Type[] { typeof(Reduction) });
			return nodeConstructor.Invoke(new object[] { reduction });
		}

		#endregion

		#region NodeFactory members

		private readonly Reduction reduction;

		/// <summary>
		/// Gets the associated reduction.
		/// </summary>
		/// <value>The reduction.</value>
		protected Reduction Reduction
		{
			get { return reduction; }
		}

		public NodeFactory(Reduction reduction)
		{
			this.reduction = reduction;
		}

		/// <summary>
		/// Builds up nodes that corresponds to that reduction (reduction 1->N nodes).
		/// </summary>
		/// <returns></returns>
		public abstract IList<Node> BuildUpNodes();

		#endregion
	}
}
