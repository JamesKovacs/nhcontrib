namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	/// <summary>
	/// Represents a string literal
	/// </summary>
	public class LiteralNode : TerminalNode
	{
		private readonly string text;

		public LiteralNode(string text) : base(null)
		{
			this.text = text;
		}

		/// <summary>
		/// Gets the literal text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return text; }
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", base.ToString(), text);
		}
	}
}