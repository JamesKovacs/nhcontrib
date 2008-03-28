namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System;
	using GoldParser;

	/// <summary>
	/// Represents a "X between A and B"
	/// </summary>
	public class BetweenOperator : Node, IOperator
	{
		public BetweenOperator(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
			if (string.Equals(sectionToInterpret.GetToken(1).Name, "not", StringComparison.InvariantCultureIgnoreCase))
			{
				// Remove the NOT keyword (if it's not between)
				sectionToInterpret.Tokens.RemoveAt(1);
			}
			if (string.Equals(sectionToInterpret.GetToken(1).Name, "between", StringComparison.InvariantCultureIgnoreCase))
			{
				// Remove the BETWEEN keyword (if it's not between)
				sectionToInterpret.Tokens.RemoveAt(1);
			}
			if (string.Equals(sectionToInterpret.GetToken(2).Name, "and", StringComparison.InvariantCultureIgnoreCase))
			{
				// Remove the AND keyword
				sectionToInterpret.Tokens.RemoveAt(2);
			}
		}

		#region Properties

		public Node FixtureOperand
		{
			get { return ChildNodes[0]; }
		}

		public Node LowOperand
		{
			get { return ChildNodes[1]; }
		}

		public Node HighOperand
		{
			get { return ChildNodes[2]; }
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{0}: <{1}> between <{2}> and <{3}>", base.ToString(), FixtureOperand, LowOperand, HighOperand);
		}
	}
}
