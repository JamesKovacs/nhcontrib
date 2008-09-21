namespace NHibernate.ProxyGenerators.Castle
{
	using System;
	using System.Reflection;

	[Serializable]
	public class CastleProxyGeneratorOptions : ProxyGeneratorOptions
	{
		public CastleProxyGeneratorOptions()
		{
		}

		public CastleProxyGeneratorOptions(string outputAssemblyPath)
			: base(outputAssemblyPath, null)
		{
		}

		public CastleProxyGeneratorOptions(string outputAssemblyPath, params Assembly[] inputAssemblies)
			: base(outputAssemblyPath, inputAssemblies)
		{
		}

		[Argument(ArgumentType.AtMostOnce, HelpText = "Path to the intermediate file used to generate the Castle proxies", DefaultValue = "CastleDynProxy2.dll", ShortName = "ip")]
		public string IntermediateProxyAssemblyPath;

		[Argument(ArgumentType.AtMostOnce, HelpText = "Path to the intermediate file used to generate the Castle Proxy Factory", DefaultValue = "CastleStaticProxyFactory.dll", ShortName = "if")]
		public string IntermediateCastleStaticProxyFactoryAssemblyPath;
	}
}