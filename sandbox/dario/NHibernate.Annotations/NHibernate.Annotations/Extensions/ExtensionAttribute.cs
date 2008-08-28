namespace System.Runtime.CompilerServices
{
	/// <summary>
	/// Fooling the compiles in order to support extension methods in .Net 2.0
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
	public sealed class ExtensionAttribute : Attribute
	{
	}
}