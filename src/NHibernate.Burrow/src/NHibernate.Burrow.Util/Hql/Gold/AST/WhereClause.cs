namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// Represents the where clause in any statement, contains all the predicates
	/// </summary>
	public class WhereClause : Node
	{
		public WhereClause(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
		}

		protected override IEnumerable<Token> UsefulChildTokens
		{
			get
			{
				// Return all tokens except the first one (the "where" string)
				bool firstItem = true;
				foreach (Token token in base.UsefulChildTokens)
				{
					if (firstItem)
					{
						firstItem = false;
					}
					else
					{
						yield return token;
					}
				}
			}
		}
	}
}