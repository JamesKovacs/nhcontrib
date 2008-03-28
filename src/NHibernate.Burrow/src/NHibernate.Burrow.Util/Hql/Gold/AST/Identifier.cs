namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	/// <summary>
	/// Represents a final identifier with no child nodes.
	/// </summary>
	public class Identifier : TerminalNode
	{
		public Identifier(string name) : base(null)
		{
			this.name = name;
		}

		private readonly string name;

		/// <summary>
		/// Gets the identifier text.
		/// </summary>
		/// <value>The node data.</value>
		public string Name
		{
			get { return name; }
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", base.ToString(), name);
		}
	}
}