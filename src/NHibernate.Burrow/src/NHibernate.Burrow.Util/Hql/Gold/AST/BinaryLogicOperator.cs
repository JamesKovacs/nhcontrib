namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// Represents logical operators (ie: and, or...)
	/// </summary>
	public class BinaryLogicOperator : BinaryOperator
	{
		public BinaryLogicOperator(Reduction sectionToInterpret)
			: base(sectionToInterpret)
		{
			Token op = (Token)sectionToInterpret.Tokens[1];
			operatorText = op.Data.ToString();
		}

		private readonly string operatorText;
		/// <summary>
		/// Gets the operator text
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