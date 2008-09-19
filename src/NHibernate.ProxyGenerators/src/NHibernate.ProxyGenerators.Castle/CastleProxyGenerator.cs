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
	public class CastleProxyGenerator : IProxyGenerator
	{
		public Assembly Generate( ProxyGeneratorOptions options )
		{
			if (options == null) throw new ProxyGeneratorException("options is Required");

			string outputAssemblyPath = options.OutputAssemblyPath;
			Assembly[] inputAssemblies = options.InputAssemblies;

			if (string.IsNullOrEmpty(outputAssemblyPath)) throw new ProxyGeneratorException("options.OutputAssemblyPath is Required");

			if (!Path.IsPathRooted(outputAssemblyPath))
			{
				outputAssemblyPath = Path.GetFullPath(outputAssemblyPath);
			}

			if (inputAssemblies == null || inputAssemblies.Length == 0) throw new ProxyGeneratorException("At least one input assembly is required");

			Configuration nhibernateConfiguration = CreateNHibernateConfiguration(inputAssemblies, options );
			if (nhibernateConfiguration.ClassMappings.Count == 0) FailNoClassMappings(inputAssemblies);

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
				throw new ProxyGeneratorException(errors.ToString());
			}

			return MergeStaticProxyFactoryWithProxies(result.CompiledAssembly, proxyResult.Assembly, inputAssemblies, outputAssemblyPath);
		}

		public virtual ProxyGeneratorOptions GetOptions()
		{
			return new ProxyGeneratorOptions();
		}

		protected virtual void FailNoClassMappings( Assembly[] inputAssemblies )
		{
			StringBuilder builder = new StringBuilder("No NHibernate Class Mappings found in inputAssemblies { ");
			for( int i=0; i<inputAssemblies.Length; i++)
			{
				builder.Append("\"");
				builder.Append(inputAssemblies[i].Location);
				builder.Append("\"");
				if( i != inputAssemblies.Length - 1 ) builder.Append(" , ");
			}
			builder.Append(" }");
			throw new ProxyGeneratorException(builder.ToString());
		}

		protected virtual Configuration CreateNHibernateConfiguration( Assembly[] inputAssemblies, ProxyGeneratorOptions options )
		{
			Configuration nhibernateConfiguration = new Configuration();

			nhibernateConfiguration.AddProperties(GetDefaultNHibernateProperties());

			foreach (Assembly inputAssembly in inputAssemblies)
			{
				nhibernateConfiguration.AddAssembly(inputAssembly);
			}

			return nhibernateConfiguration;
		}

		protected virtual IDictionary<string, string> GetDefaultNHibernateProperties()
		{
			Dictionary<string, string> properties = new Dictionary<string, string>();
			properties["cache.provider_class"] = typeof(HashtableCacheProvider).AssemblyQualifiedName;
			properties["dialect"] = typeof(MsSql2000Dialect).AssemblyQualifiedName;
			properties["proxyfactory.factory_class"] = typeof(CastleProxyFactoryFactory).AssemblyQualifiedName;
			return properties;
		}

		protected virtual GenerateProxiesResult GenerateProxies(Configuration nhibernateConfiguration)
		{
			ModuleScope moduleScope = new ModuleScope(true);
			IDictionary proxies = new Hashtable();

			try
			{
				CastleProxyFactoryFactory.ProxyFactory = new CastleProxyFactory(new DefaultProxyBuilder(moduleScope), proxies);
				using (nhibernateConfiguration.BuildSessionFactory())
				{
				}
			}
			finally
			{
				CastleProxyFactoryFactory.ProxyFactory = null;
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
			Type castleProxyGeneratorType = typeof(CastleProxyGenerator);
			using (Stream stream = castleProxyGeneratorType.Assembly.GetManifestResourceStream(castleProxyGeneratorType, typeof(CastleStaticProxyFactory).Name + ".cs"))
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
			parameters.OutputAssembly = typeof(CastleStaticProxyFactory).Name + ".dll";
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