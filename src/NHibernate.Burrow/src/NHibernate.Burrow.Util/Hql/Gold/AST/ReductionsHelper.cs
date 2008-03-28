namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using System.Collections.Generic;
	using System.Text;
	using GoldParser;

	/// <summary>
	/// Contains helper methods to simplify reductions
	/// </summary>
	public static class ReductionsHelper
	{
		#region FlatternChilds

		/// <summary>
		/// Flatterns the childs removing the sections given in flatternSections.
		/// </summary>
		/// <param name="reduction">The reduction.</param>
		/// <param name="flatternSections">The flattern sections.</param>
		public static IEnumerable<Token> FlatternChilds(Reduction reduction, params string[] flatternSections)
		{
			List<string> sectionsToFlattern = new List<string>(flatternSections);
			foreach (Token token in reduction.Tokens)
			{
				Reduction tokenReduction = token.Data as Reduction;
				string tokenName = tokenReduction != null ? tokenReduction.ParentRule.RuleNonTerminal.Name : null;
				if (!sectionsToFlattern.Contains(tokenName))
				{
					// if the token doesn't need to be processed, return it
					yield return token;
				}
				else if (token.Kind == SymbolType.NonTerminal)
				{
					// Flattern the child
					foreach (Token child in FlatternChilds(token.Data as Reduction, flatternSections))
					{
						yield return child;
					}
				}
				else
				{
					// The child doesn't contains child levels because is a Terminal symbol
					yield return token;
				}
			}
		}

		#endregion

		#region Merge Childs

		public static void MergeChilds(Reduction sectionToInterpret, string separator)
		{
			MergeChilds(sectionToInterpret, separator, new int[0]);
		}

		public static void MergeChilds(Reduction sectionToInterpret, string separator, int[] childsToMerge)
		{
			List<int> items = new List<int>();

			// If all items have to needs to be merged, create a list of all indexes
			if (childsToMerge.Length == 0)
			{
				// Merge all
				for (int i = 0; i < sectionToInterpret.Tokens.Count; i++)
					items.Add(i);
			}
			else
			{
				items.AddRange(childsToMerge);
				items.Sort();
			}

			// Validate that all given indexes are inbound.
			int maxValue = items[items.Count - 1];
			if (sectionToInterpret.Tokens.Count > maxValue)
			{
				int itemsMerged = 0;

				Token baseToken = (Token)sectionToInterpret.Tokens[items[0]];

				// Must ensure that this token is not a non-terminal, if it's a NonT merge all the subitems
				Reduction baseReduction = baseToken.Data as Reduction;
				if (baseReduction != null)
				{
					baseToken.Data = MergeAllChilds(baseReduction, separator);
					baseToken.Kind = SymbolType.Terminal;
				}


				for (int c = 1; c < items.Count; c++)
				{
					int item = items[c - itemsMerged];

					// If a non-terminal section is trying to be merged, merge the subitems first
					Reduction reduction = ((Token)sectionToInterpret.Tokens[item]).Data as Reduction;
					if (reduction != null)
					{
						((Token)sectionToInterpret.Tokens[item]).Data = MergeAllChilds(reduction, separator);
						baseToken.Kind = SymbolType.Terminal;
					}

					baseToken.Data = baseToken.Data + separator + ((Token)sectionToInterpret.Tokens[item]).Data;

					sectionToInterpret.Tokens.RemoveAt(item);

					itemsMerged++;
				}
			}
		}

		private static string MergeAllChilds(Reduction reduction, string separator)
		{
			StringBuilder mergedText = new StringBuilder();
			while (reduction.Tokens.Count > 0)
			{
				object data = ((Token)reduction.Tokens[0]).Data;
				Reduction subReduction = data as Reduction;

				if (subReduction == null)
				{
					// If it's a simple terminal, append it
					if (mergedText.Length > 0)
						mergedText.Append(separator);

					mergedText.Append(data);
				}
				else
				{
					// If it's a non-terminal, convert all childs to text
					mergedText.Append(MergeAllChilds(subReduction, separator));
				}

				// Remove the first item and continue with the others
				reduction.Tokens.RemoveAt(0);
			}

			return mergedText.ToString();
		}

		#endregion
	}
}
