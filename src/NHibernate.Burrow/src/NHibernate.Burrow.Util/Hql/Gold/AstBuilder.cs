namespace NHibernate.Burrow.Util.Hql.Gold
{
	using System.Text;
	using GoldParser;
	using AST;
	using log4net;
	using log4net.Config;

	/// <summary>
	/// Creates the abstract sintax tree parsing the query with GOLD
	/// </summary>
	public static class AstBuilder
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(AstBuilder));

		static AstBuilder()
		{
			XmlConfigurator.Configure();
		}

		/// <summary>
		/// Gets the tree root.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		public static Node GetTreeRoot(string query)
		{
			if (log.IsDebugEnabled)
			{
				log.DebugFormat("Building AST for '{0}'", query);
			}

			HqlParser parser = new HqlParser();
			Reduction rootReduction = parser.Execute(query);

			// Get the correct root node (always should start with a Statement)
			object manager = NodeFactory.GetManagerByReduction(rootReduction);
			IStatement statement;
			statement = manager as IStatement;

			if (statement == null)
			{
				// Right now, the Select Statement is the only that can start in many ways. 
				// So hardcode the select statement and tell it to parse the root found as childs.
				statement = new SelectStatement(manager);
			}

			Node root = statement as Node;
			if (log.IsDebugEnabled)
			{
				OutputASTRecursively(root, 0);
			}

			return root;
		}

		/// <summary>
		/// Prints out the AST, starting from the given root node.
		/// </summary>
		/// <param name="root">The root.</param>
		/// <param name="level">The level.</param>
		private static void OutputASTRecursively(Node root, int level)
		{
			// Log the tree
			StringBuilder tabbify = new StringBuilder();
			for (int i = 0; i < level; i++)
			{
				tabbify.Append("|  ");
			}
			tabbify.Append("+--");
			log.Debug(tabbify.ToString() + root);

			foreach (Node node in root.ChildNodes)
			{
				OutputASTRecursively(node, level + 1);
			}
		}
	}
}