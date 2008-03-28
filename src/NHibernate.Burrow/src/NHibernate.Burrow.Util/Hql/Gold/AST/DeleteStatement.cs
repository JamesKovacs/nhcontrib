namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System;
	using GoldParser;

	/// <summary>
	/// Represents a hql delete statement, with all included
	/// </summary>
	public class DeleteStatement : Node, IStatement
	{
		public DeleteStatement(Reduction reduction)
			: base(reduction)
		{
			// Remove the "delete from" tokens
			if (string.Equals(reduction.GetToken(0).Name, "delete", StringComparison.InvariantCultureIgnoreCase))
			{
				reduction.Tokens.RemoveAt(0);
				reduction.Tokens.RemoveAt(0);
			}
		}
	}
}