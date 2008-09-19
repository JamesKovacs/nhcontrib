namespace NHibernate.ProxyGenerators
{
	using System.Reflection;

	public class ProxyGeneratorOptions
	{
		public ProxyGeneratorOptions()
		{
		}

		public ProxyGeneratorOptions(string outputAssemblyPath)
			: this(outputAssemblyPath, null)
		{
		}

		public ProxyGeneratorOptions(string outputAssemblyPath, params Assembly[] inputAssemblies)
		{
			OutputAssemblyPath = outputAssemblyPath;
			InputAssemblies = inputAssemblies;
		}

		[Argument(ArgumentType.AtMostOnce, HelpText="The Proxy Generator to use.  One of [castle|activerecord] or Assembly Qualified Name", DefaultValue="castle", ShortName="g")]
		public string Generator;

		[Argument(ArgumentType.Required, HelpText = "Path to output assembly for generated proxies.  e.g. .\\OutputAssembly.dll", ShortName="o")]
		public string OutputAssemblyPath;

		[DefaultArgument(ArgumentType.MultipleUnique, HelpText = "Path to assembly(ies) containing NHibernate Class Mappings")]
		public string[] InputAssemblyPaths;

		private Assembly[] _inputAssemblies;
		public Assembly[] InputAssemblies
		{
			get { return _inputAssemblies; }
			set { _inputAssemblies = value; }
		}
	}
}