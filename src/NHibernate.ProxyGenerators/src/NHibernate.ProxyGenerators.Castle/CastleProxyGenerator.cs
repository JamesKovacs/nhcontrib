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
		public Assembly Generate(ProxyGeneratorOptions options)
		{
			CastleProxyGeneratorOptions castleOptions = ValidateOptions(options);

			try
			{
				CrossAppDomainCaller.RunInOtherAppDomain(Generate, this, castleOptions);	
			}
			finally
			{
				CleanUpIntermediateFiles(castleOptions);	
			}
			
			return Assembly.LoadFrom(options.OutputAssemblyPath);
		}

		public virtual ProxyGeneratorOptions GetOptions()
		{
			return new CastleProxyGeneratorOptions();
		}

		public CastleProxyGeneratorOptions ValidateOptions(ProxyGeneratorOptions options)
		{
			CastleProxyGeneratorOptions castleOptions = options as CastleProxyGeneratorOptions;

			if (castleOptions == null) throw new ProxyGeneratorException("options must be of type {0}", typeof(CastleProxyGeneratorOptions).Name);

			if (string.IsNullOrEmpty(castleOptions.OutputAssemblyPath)) throw new ProxyGeneratorException("options.OutputAssemblyPath is Required");

			if (!Path.IsPathRooted(castleOptions.OutputAssemblyPath))
			{
				castleOptions.OutputAssemblyPath = Path.GetFullPath(castleOptions.OutputAssemblyPath);
			}

			if (castleOptions.InputAssemblyPaths == null || castleOptions.InputAssemblyPaths.Length == 0) throw new ProxyGeneratorException("At least one input assembly is required");

			if (string.IsNullOrEmpty(castleOptions.IntermediateProxyAssemblyPath))
			{
				castleOptions.IntermediateProxyAssemblyPath = ModuleScope.DEFAULT_FILE_NAME;
			}

			if (string.IsNullOrEmpty(castleOptions.IntermediateCastleStaticProxyFactoryAssemblyPath))
			{
				castleOptions.IntermediateCastleStaticProxyFactoryAssemblyPath = typeof(CastleStaticProxyFactory).Name + ".dll";
			}

			return castleOptions;
		}

		private static void Generate(object[] args)
		{
			CastleProxyGenerator proxyGenerator = (CastleProxyGenerator)args[0];
			CastleProxyGeneratorOptions generatorOptions = (CastleProxyGeneratorOptions)args[1];

			using (AssemblyResolver resolver = new AssemblyResolver(generatorOptions.InputAssemblyPaths))
			{
				generatorOptions.InputAssemblies = resolver.LoadFrom(generatorOptions.InputAssemblyPaths);
				proxyGenerator.Generate(generatorOptions);
			}
		}

		protected virtual void Generate( CastleProxyGeneratorOptions options )
		{
			string outputAssemblyPath = options.OutputAssemblyPath;

			Assembly[] inputAssemblies = options.InputAssemblies;

			Configuration nhibernateConfiguration = CreateNHibernateConfiguration(inputAssemblies, options );
			if (nhibernateConfiguration.ClassMappings.Count == 0) FailNoClassMappings(inputAssemblies);

			GenerateProxiesResult proxyResult = GenerateProxies(nhibernateConfiguration, options.IntermediateProxyAssemblyPath);

			string staticProxyFactorySourceCode = GenerateStaticProxyFactorySourceCode(proxyResult.Proxies, inputAssemblies[0].GetName().Version);

			CompilerResults result = CompileStaticProxyFactory(nhibernateConfiguration, proxyResult.Assembly, staticProxyFactorySourceCode, options.IntermediateCastleStaticProxyFactoryAssemblyPath);

			if (result.Errors.HasErrors)
			{
				StringBuilder errors = new StringBuilder();
				foreach (CompilerError error in result.Errors)
				{
					errors.AppendLine(error.ToString());
				}
				throw new ProxyGeneratorException(errors.ToString());
			}

			MergeStaticProxyFactoryWithProxies(result.CompiledAssembly, proxyResult.Assembly, inputAssemblies, outputAssemblyPath);
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

			//nhibernateConfiguration.AddProperties(GetDefaultNHibernateProperties(options));
			nhibernateConfiguration.SetProperties(GetDefaultNHibernateProperties(options));

			foreach (Assembly inputAssembly in inputAssemblies)
			{
				nhibernateConfiguration.AddAssembly(inputAssembly);
			}

			return nhibernateConfiguration;
		}

		protected virtual IDictionary<string, string> GetDefaultNHibernateProperties(ProxyGeneratorOptions options)
		{
			Dictionary<string, string> properties = new Dictionary<string, string>();
			properties["cache.provider_class"] = typeof(HashtableCacheProvider).AssemblyQualifiedName;
			properties["dialect"] = options.Dialect;
			properties["proxyfactory.factory_class"] = typeof(CastleProxyFactoryFactory).AssemblyQualifiedName;
			return properties;
		}

		protected virtual GenerateProxiesResult GenerateProxies(Configuration nhibernateConfiguration, string modulePath)
		{
			ModuleScope moduleScope = new ModuleScope(true, ModuleScope.DEFAULT_ASSEMBLY_NAME, modulePath, ModuleScope.DEFAULT_ASSEMBLY_NAME, modulePath );
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
			proxyAssemblyName.CodeBase = modulePath;

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

		protected virtual CompilerResults CompileStaticProxyFactory(Configuration nhibernateConfiguration, Assembly proxyAssembly, string sourceCode, string outputAssembly)
		{
			CompilerParameters parameters = new CompilerParameters();
			parameters.OutputAssembly = outputAssembly;
			parameters.WarningLevel = 4;
			parameters.TreatWarningsAsErrors = true;
			parameters.CompilerOptions = "/debug:pdbonly /optimize+";

			List<Assembly> references = new List<Assembly>();
			references.Add(Assembly.Load("NHibernate"));
			references.Add(Assembly.Load("Iesi.Collections"));
			references.Add(Assembly.Load("log4net"));
			references.Add(Assembly.Load("Castle.Core"));
			references.Add(Assembly.Load("NHibernate.ProxyGenerators.CastleDynamicProxy"));
			references.Add(proxyAssembly);

			AssemblyName[] proxyReferencedAssemblyNames = proxyAssembly.GetReferencedAssemblies();
			foreach( AssemblyName proxyReferencedAssemblyName in proxyReferencedAssemblyNames )
			{
				Assembly proxyReferencedAssembly = Assembly.Load(proxyReferencedAssemblyName);
				if( !references.Contains(proxyReferencedAssembly) )
				{
					references.Add(proxyReferencedAssembly);
				}
			}

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

		protected virtual void MergeStaticProxyFactoryWithProxies(Assembly staticProxyAssembly, Assembly proxyAssembly, Assembly[] referenceAssemblies, string outputPath)
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
		}

		protected virtual void CleanUpIntermediateFiles(CastleProxyGeneratorOptions castleOptions)
		{
			if (File.Exists(castleOptions.IntermediateProxyAssemblyPath))
			{
				File.Delete(castleOptions.IntermediateProxyAssemblyPath);
			}

			string intermediateProxyAssemblyPdbPath = Path.ChangeExtension(castleOptions.IntermediateProxyAssemblyPath, "pdb");
			if (File.Exists(intermediateProxyAssemblyPdbPath))
			{
				File.Delete(intermediateProxyAssemblyPdbPath);
			}

			if (File.Exists(castleOptions.IntermediateCastleStaticProxyFactoryAssemblyPath))
			{
				File.Delete(castleOptions.IntermediateCastleStaticProxyFactoryAssemblyPath);
			}

			string intermediateCastleStaticProxyFactoryAssemblyPdbPath = Path.ChangeExtension(castleOptions.IntermediateCastleStaticProxyFactoryAssemblyPath, "pdb");
			if (File.Exists(intermediateCastleStaticProxyFactoryAssemblyPdbPath))
			{
				File.Delete(intermediateCastleStaticProxyFactoryAssemblyPdbPath);
			}
		}
	}
}
