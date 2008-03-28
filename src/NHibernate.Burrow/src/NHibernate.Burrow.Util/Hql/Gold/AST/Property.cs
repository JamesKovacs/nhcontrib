namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	/// <summary>
	/// Represents a complex propery or identifier, for example: Something.Other.Thing
	/// </summary>
	public class Property : TerminalNode
	{
		public Property(string name)
			: base(null)
		{
			this.name = name;
		}

		private readonly string name;

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		/// <value>The name.</value>
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