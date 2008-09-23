namespace NHibernate.ProxyGenerators
{
	using System;
	using System.Reflection;

	[Serializable]
	public class ProxyGeneratorOptions
	{
		public ProxyGeneratorOptions()
		{
		}

		public ProxyGeneratorOptions(string outputAssemblyPath)
			: this(outputAssemblyPath, null)
		{
		}

		public ProxyGeneratorOptions(string outputAssemblyPath, params string[] inputAssemblyPaths)
		{
			OutputAssemblyPath = outputAssemblyPath;
			InputAssemblyPaths = inputAssemblyPaths;
			Dialect = "NHibernate.Dialect.MsSql2005Dialect";
		}

		[Argument(ArgumentType.AtMostOnce, HelpText="The Proxy Generator to use.  One of [castle|activerecord] or Assembly Qualified Name", DefaultValue="castle", ShortName="g")]
		public string Generator;

		[Argument(ArgumentType.Required, HelpText = "Path to output assembly for generated proxies.  e.g. .\\OutputAssembly.dll", ShortName="o")]
		public string OutputAssemblyPath;

		[DefaultArgument(ArgumentType.MultipleUnique, HelpText = "Path to assembly(ies) containing NHibernate Class Mappings")]
		public string[] InputAssemblyPaths;

		[Argument(ArgumentType.AtMostOnce, HelpText = "The dialect to use. Only needed when you use dialect specific mapping options (like sequences in Oracle).", DefaultValue = "NHibernate.Dialect.MsSql2005Dialect", ShortName = "d")]
		public string Dialect;
	}
}
