namespace NHibernate.ProxyGenerators
{
	public class ProxyGeneratorOptions
	{
		[Argument(ArgumentType.Required, HelpText = "Rooted path to output assembly for generated proxies.  e.g. C:\\OutputAssembly.dll", ShortName="out")]
		public string OutputAssemblyPath;

		[DefaultArgument(ArgumentType.MultipleUnique, HelpText = "Path to assembly(ies) containing NHibernate Class Mappings")]
		public string[] InputAssemblyPaths;
	}
}