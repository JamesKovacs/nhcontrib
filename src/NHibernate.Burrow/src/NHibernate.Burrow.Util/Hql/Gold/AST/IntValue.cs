namespace NHibernate.Burrow.Util.Hql.Gold.AST
{
	/// <summary>
	/// Represents a simple int or numeric value
	/// </summary>
	public class IntValue : TerminalNode
	{
		#region Ctor

		public IntValue(string intValue)
			: base(null)
		{
			this.intValue = int.Parse(intValue);
		}

		public IntValue(int intValue)
			: base(null)
		{
			this.intValue = intValue;
		}

		#endregion

		#region Properties

		private readonly int intValue;
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public int Value
		{
			get { return intValue; }
		}

		#endregion

		#region Operators

		/// <summary>
		/// Implicit cast between an IntValue and the Int32
		/// </summary>
		/// <returns></returns>
		public static implicit operator int(IntValue iv)
		{
			return iv.intValue;
		}

		/// <summary>
		/// Implicit cast between an Int32 and an IntValue
		/// </summary>
		/// <returns></returns>
		public static implicit operator IntValue(int iv)
		{
			return new IntValue(iv);
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{0}: {1}", base.ToString(), Value);
		}
	}
}