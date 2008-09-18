namespace NHibernate.ProxyGenerators.Castle
{
	using System;
	using System.CodeDom.Compiler;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Text;
	using Cache;
	using Cfg;
	using Dialect;
	using global::Castle.DynamicProxy;
	using ILMerging;
	using Mapping;
	using Microsoft.CSharp;

	[Serializable]
	public class CaslteProxyGenerator : IProxyGenerator
	{
		public Assembly Generate( string outputAssemblyPath, params Assembly[] inputAssemblies )
		{
			Configuration nhibernateConfiguration = CreateNHibernateConfiguration(inputAssemblies);

			GenerateProxiesResult proxyResult = GenerateProxies(nhibernateConfiguration);

			string staticProxyFactorySourceCode = GenerateStaticProxyFactorySourceCode(proxyResult.Proxies, inputAssemblies[0].GetName().Version);

			CompilerResults result = CompileStaticProxyFactory(nhibernateConfiguration, proxyResult.Assembly, staticProxyFactorySourceCode);

			if (result.Errors.HasErrors)
			{
				StringBuilder errors = new StringBuilder();
				foreach (CompilerError error in result.Errors)
				{
					errors.AppendLine(error.ToString());
				}
				throw new Exception(errors.ToString());
			}

			return MergeStaticProxyFactoryWithProxies(result.CompiledAssembly, proxyResult.Assembly, inputAssemblies, outputAssemblyPath);
		}

		protected virtual Configuration CreateNHibernateConfiguration(Assembly[] inputAssemblies)
		{
			Configuration nhibernateConfiguration = new Configuration();
			nhibernateConfiguration.Properties["cache.provider_class"] = typeof(HashtableCacheProvider).AssemblyQualifiedName;
			nhibernateConfiguration.Properties["dialect"] = typeof(MsSql2000Dialect).AssemblyQualifiedName;
			nhibernateConfiguration.Properties["proxyfactory.factory_class"] = typeof(GeneratorProxyFactoryFactory).AssemblyQualifiedName;

			foreach (Assembly inputAssembly in inputAssemblies)
			{
				nhibernateConfiguration.AddAssembly(inputAssembly);
			}

			return nhibernateConfiguration;
		}

		protected virtual GenerateProxiesResult GenerateProxies(Configuration nhibernateConfiguration)
		{
			ModuleScope moduleScope = new ModuleScope(true);
			IDictionary proxies = new Hashtable();

			try
			{
				GeneratorProxyFactoryFactory.ProxyFactory = new CastleProxyFactory(new DefaultProxyBuilder(moduleScope), proxies);
				using (nhibernateConfiguration.BuildSessionFactory())
				{
				}
			}
			finally
			{
				GeneratorProxyFactoryFactory.ProxyFactory = null;
			}

			moduleScope.SaveAssembly();
			moduleScope = null;

			AssemblyName proxyAssemblyName = new AssemblyName(ModuleScope.DEFAULT_ASSEMBLY_NAME);
			proxyAssemblyName.CodeBase = ModuleScope.DEFAULT_FILE_NAME;

			Assembly proxyAssembly = Assembly.Load(proxyAssemblyName);

			return new GenerateProxiesResult(proxies, proxyAssembly);
		}

		protected class GenerateProxiesResult
		{
			public readonly IDictionary Proxies;
			public readonly Assembly Assembly;

			public GenerateProxiesResult(IDictionary proxies, Assembly assembly)
			{
				Proxies = proxies;
				Assembly = assembly;
			}
		}

		protected virtual string GenerateStaticProxyFactorySourceCode(IDictionary proxies, Version sourceVersion)
		{
			StringBuilder proxyCode = new StringBuilder();
			foreach (DictionaryEntry entry in proxies)
			{
				proxyCode.AppendFormat("\t\t_proxies[\"{0}\"] = typeof({1});\r\n", entry.Key, ((Type)entry.Value).Name);
			}

			string source;
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.ProxyGenerators.Castle.StaticProxyFactory.cs"))
			{
				using (TextReader reader = new StreamReader(stream))
				{
					source = reader.ReadToEnd();
				}
				source = source.Replace("//", string.Empty);
				source = source.Replace("{VERSION}", sourceVersion.ToString());
				source = source.Replace("{PROXIES}", proxyCode.ToString());
			}

			return source;
		}

		protected virtual CompilerResults CompileStaticProxyFactory(Configuration nhibernateConfiguration, Assembly proxyAssembly, string sourceCode)
		{
			CompilerParameters parameters = new CompilerParameters();
			parameters.OutputAssembly = "StaticProxyFactory.dll";
			parameters.WarningLevel = 4;
			parameters.TreatWarningsAsErrors = true;
			parameters.CompilerOptions = "/debug:pdbonly /optimize+";

			List<Assembly> references = new List<Assembly>();
			references.Add(Assembly.Load("NHibernate"));
			references.Add(Assembly.Load("Iesi.Collections"));
			references.Add(Assembly.Load("log4net"));
			references.Add(Assembly.Load("Castle.Core"));
			references.Add(proxyAssembly);
			foreach (PersistentClass cls in nhibernateConfiguration.ClassMappings)
			{
				if (!references.Contains(cls.MappedClass.Assembly))
				{
					references.Add(cls.MappedClass.Assembly);
				}
			}

			foreach (Assembly assembly in references)
			{
				parameters.ReferencedAssemblies.Add(assembly.Location);
			}

			CSharpCodeProvider compiler = new CSharpCodeProvider();
			return compiler.CompileAssemblyFromSource(parameters, sourceCode);
		}

		protected virtual Assembly MergeStaticProxyFactoryWithProxies(Assembly staticProxyAssembly, Assembly proxyAssembly, Assembly[] referenceAssemblies, string outputPath)
		{
			ILMerge merger = new ILMerge();

			List<string> searchDirectories = new List<string>(referenceAssemblies.Length);
			foreach(Assembly referenceAssembly in referenceAssemblies )
			{
				string searchDirectory = Path.GetDirectoryName(referenceAssembly.Location);
				if (!searchDirectories.Contains(searchDirectory))
				{
					searchDirectories.Add(searchDirectory);
				}
			}

			merger.SetSearchDirectories(searchDirectories.ToArray());
			merger.SetInputAssemblies(new string[] { staticProxyAssembly.Location, proxyAssembly.Location });
			merger.OutputFile = outputPath;
			merger.Merge();

			return Assembly.LoadFile(outputPath);
		}
	}
}