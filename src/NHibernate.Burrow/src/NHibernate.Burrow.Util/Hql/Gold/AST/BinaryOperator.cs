namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// Represents all kind of binary operators (ie: like, =, &gt;=, in, and, or, etc...)
	/// </summary>
	public abstract class BinaryOperator : Node, IOperator
	{
		public BinaryOperator(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
		}

		protected override IEnumerable<Token> UsefulChildTokens
		{
			get
			{
				// Return all but the operator
				yield return SectionToInterpret.GetToken(0);
				yield return SectionToInterpret.GetToken(2);
			}
		}

		#region Properties

		/// <summary>
		/// Gets the left hand operand.
		/// </summary>
		/// <value>The left hand operand.</value>
		public virtual Node LeftHandOperand
		{
			get
			{
				return base.ChildNodes[0];
			}
		}

		/// <summary>
		/// Gets the right hand operand.
		/// </summary>
		/// <value>The right hand operand.</value>
		public virtual Node RightHandOperand
		{
			get
			{
				// Note: The operator is never returned in BinaryOperators
				return base.ChildNodes[1];
			}
		}

		#endregion
	}
}