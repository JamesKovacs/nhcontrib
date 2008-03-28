namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	using GoldParser;

	/// <summary>
	/// The parameter type, might be set using a question mark (?), or a colon before the name
	/// </summary>
	public enum ParameterType
	{
		Positional,
		Named
	}

	/// <summary>
	/// Represents a parameter in a query (can be either positional or named parameter)
	/// </summary>
	public class Parameter : TerminalNode
	{
		public Parameter(Reduction sectionToInterpret) : base(sectionToInterpret)
		{
			string prefix = sectionToInterpret.GetToken(0).Data.ToString();
			if (prefix == "?")
				type = ParameterType.Positional;
			else
			{
				type = ParameterType.Named;
				name = sectionToInterpret.GetToken(1).Data.ToString();
			}
		}

		private readonly ParameterType type;
		/// <summary>
		/// Gets the parameter type.
		/// </summary>
		/// <value>The type.</value>
		public ParameterType Type
		{
			get { return type; }
		}

		private readonly string name;
		/// <summary>
		/// Gets the parameter name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return name; }
		}

		public override string ToString()
		{
			string formatedName = Type == ParameterType.Named ? (" (Name: " + Name + ")") : "";
			return string.Format("{0}: {1}{2}", base.ToString(), Type, formatedName);
		}
	}
}