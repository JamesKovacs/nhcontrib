namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using GoldParser;

	/// <summary>
	/// Represents an arithmetic operator (like =, >=, like, ...)
	/// </summary>
	public class BinaryArithmeticOperator : BinaryOperator
	{
		private readonly string operatorText;

		public BinaryArithmeticOperator(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
			// Ignore not
			if (sectionToInterpret.GetToken(1).Name == "not")
				sectionToInterpret.Tokens.RemoveAt(1);

			Token op = (Token) sectionToInterpret.Tokens[1];
			operatorText = op.Data.ToString();
		}

		/// <summary>
		/// Gets the operator text (=, >=, !=, like, ...)
		/// </summary>
		/// <value>The operator text.</value>
		public string OperatorText
		{
			get { return operatorText; }
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", base.ToString(), operatorText);
		}
	}
}