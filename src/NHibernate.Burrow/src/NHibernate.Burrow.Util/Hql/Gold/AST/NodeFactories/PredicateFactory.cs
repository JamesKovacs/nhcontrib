namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// Factory in charge of creating the corresponding predicate, given all nodes for the operator
	/// Predicates are (see grammar): 
	/// 	X between X and X
	/// 	X is [not] null
	/// 	X like X
	/// 	X in X
	/// 	X Operator X	(operators are =, >, &lt;, !=, ...) 
	/// 	'(' X ')'
	/// </summary>
	public class PredicateFactory : NodeFactory
	{
		public PredicateFactory(Reduction reduction) : base(reduction)
		{
		}

		public override IList<Node> BuildUpNodes()
		{
			// Simple pseudo-form of the predicates, to easily associate a type with the given structure
			// The string array is defined this way: if the item is null means that is variable ignored, 
			// otherwise it should be used to compare and determine the type of operator

			Dictionary<string[], CreateOperator> predicatesForm = new Dictionary<string[], CreateOperator>();
			// Unary operators
			predicatesForm.Add(new string[] { null, "is", "null" }, CreateIsNullOperator);
			predicatesForm.Add(new string[] { null, "is", "not", "null" }, CreateIsNotNullOperator);

			// Binary Operators
			predicatesForm.Add(new string[] { null, "Operator", null }, CreateBinaryArithmeticOperator);
			predicatesForm.Add(new string[] { null, "like", null }, CreateBinaryArithmeticOperator);
			predicatesForm.Add(new string[] { null, "not", "like", null }, CreateNotBinaryArithmeticOperator);
			predicatesForm.Add(new string[] { null, "in", null }, CreateBinaryArithmeticOperator);
			predicatesForm.Add(new string[] { null, "not", "in", null }, CreateNotBinaryArithmeticOperator);

			// Ternary Operator
			predicatesForm.Add(new string[] { null, "between", null, "and", null }, CreateBetweenOperator);
			predicatesForm.Add(new string[] { null, "not", "between", null, "and", null }, CreateNotBetweenOperator);

			// Complex expressions
			predicatesForm.Add(new string[] { "(", null, ")" }, CreateGroupedExpression);

			List<Node> nodes = new List<Node>(1);
			foreach (KeyValuePair<string[], CreateOperator> form in predicatesForm)
			{
				if (form.Key.Length == Reduction.Tokens.Count)
				{
					// Verify that the tokens have the same structure that the predicate form
					bool isValidForm = true;
					int index = 0;
					foreach (string value in form.Key)
					{
						// If the value is null, don't care
						if (value != null)
						{
							// Ensure that the token name is equal to the form value
							if (((Token)Reduction.Tokens[index]).Name != value)
							{
								isValidForm = false;
								break;
							}
						}
						index++;
					}

					// If the correct predicate was found..
					if (isValidForm)
					{
						nodes.Add(form.Value(Reduction));
						break;
					}
				}
			}
			return nodes;
		}

		#region Create Specific Operator

		private delegate Node CreateOperator(Reduction reduction);

		private static Node CreateIsNullOperator(Reduction reduction)
		{
			// Create the node for the given reduction, if it's a Factory I must ensure that builds only one node.
			IList<Node> nodes = GetNodesByToken(reduction.GetToken(0));
			if (nodes.Count != 1)
				throw new AstException("The 'is null' operator can contain only simple nodes.");

			return new IsNullOperator(nodes[0]);
		}

		private static Node CreateIsNotNullOperator(Reduction reduction)
		{
			return new NotOperator(CreateIsNullOperator(reduction));
		}

		private static Node CreateBinaryArithmeticOperator(Reduction reduction)
		{
			return new BinaryArithmeticOperator(reduction);
		}

		private static Node CreateNotBinaryArithmeticOperator(Reduction reduction)
		{
			return new NotOperator(CreateBinaryArithmeticOperator(reduction));
		}

		private static Node CreateBetweenOperator(Reduction reduction)
		{
			return new BetweenOperator(reduction);
		}

		private static Node CreateNotBetweenOperator(Reduction reduction)
		{
			return new NotOperator(CreateBetweenOperator(reduction));
		}

		private static Node CreateGroupedExpression(Reduction reduction)
		{
			// Create the node for the given reduction, if it's a Factory I must ensure that builds only one node.
			// The format is '(' <Expression> ')', parentheses are omitted
			IList<Node> nodes = GetNodesByToken(reduction.GetToken(1));
			if (nodes.Count != 1)
				throw new AstException("The returned expression can contain only simple nodes.");

			return nodes[0];
		}

		#endregion
	}
}