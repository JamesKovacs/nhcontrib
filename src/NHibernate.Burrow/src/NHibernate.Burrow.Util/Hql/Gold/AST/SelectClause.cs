namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System;
	using System.Collections.Generic;
	using GoldParser;

	/// <summary>
	/// The types of filters that can be present in the select clause
	/// </summary>
	public enum SelectFilter
	{
		All,
		Distinct
	}

	/// <summary>
	/// Represents the select clause inside a select statement (only the select part)
	/// </summary>
	public class SelectClause : Node
	{

		public SelectClause(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
			// Remove the "select" text
			sectionToInterpret.Tokens.RemoveAt(0);

			// Load the filters (all, distinct, top N)
			Token tokenFilter = sectionToInterpret.GetToken(0);
			if (tokenFilter.Name == "SelectFilter")
			{
				Reduction reduction = tokenFilter.Data as Reduction;
				if (reduction != null)
				{
					filter = (SelectFilter) Enum.Parse(typeof (SelectFilter), reduction.GetToken(0).Data.ToString(), true);
				}
				sectionToInterpret.Tokens.RemoveAt(0);
			}
		}

		/// <summary>
		/// Gets the childs that are important nodes, overriding this property can modify the childs while still using
		/// the NodeFactory to find the right nodes.
		/// </summary>
		/// <value>The useful child tokens.</value>
		protected override IEnumerable<Token> UsefulChildTokens
		{
			get
			{
				// Flattern the column list, and return all but the separators
				foreach (Token token in ReductionsHelper.FlatternChilds(SectionToInterpret, "ColumnList"))
				{
					if (token.Name != ",")
						yield return token;
				}
			}
		}

		private readonly SelectFilter? filter;
		/// <summary>
		/// Gets the filter applied to the select clause.
		/// </summary>
		/// <value>The filter.</value>
		public SelectFilter? Filter
		{
			get { return filter; }
		}
	}
}