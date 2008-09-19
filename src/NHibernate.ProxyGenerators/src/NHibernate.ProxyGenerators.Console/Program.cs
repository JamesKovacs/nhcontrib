namespace NHibernate.ProxyGenerators.Console
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Text;

	public class Program
	{
		private IProxyGenerator _proxyGenerator;

		public IProxyGenerator ProxyGenerator
		{
			get { return _proxyGenerator; }
			set { _proxyGenerator = value; }
		}

		public int Execute( TextWriter error, params string[] args )
		{
			ProxyGeneratorOptions generatorOptions = new ProxyGeneratorOptions();
			if (Parser.ParseArguments(args, generatorOptions) == false)
			{
				error.WriteLine(Parser.ArgumentsUsage(generatorOptions.GetType()));
				return Error.InvalidArguments;
			}

			if (_proxyGenerator == null)
			{
				try
				{
					_proxyGenerator = CreateProxyGenerator(generatorOptions.Generator);
				}
				catch (Exception exc)
				{
					error.WriteLine(exc.Message);
					return Error.CreateProxyGenerator;
				}
			}

			generatorOptions = _proxyGenerator.GetOptions();
			if( generatorOptions == null )
			{
				error.WriteLine("{0}.GetOptions() returned null.  Please use a different Generator.", _proxyGenerator.GetType().FullName);
				return Error.InvalidGenerator;
			}

			if (Parser.ParseArguments(args, generatorOptions) == false)
			{
				error.WriteLine(Parser.ArgumentsUsage(generatorOptions.GetType()));
				return Error.InvalidArguments;
			}

			IEnumerable<string> inputDirectories = GetInputDirectories(generatorOptions.InputAssemblyPaths);
			AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
			{
				foreach (string inputDirectory in inputDirectories)
				{
					Assembly inputAssembly = SearchInputDirectoryForAssembly(inputDirectory, e.Name);

					if (inputAssembly != null) return inputAssembly;
				}

				return null;
			};

			generatorOptions.InputAssemblies = LoadInputAssemblies(generatorOptions.InputAssemblyPaths, error);
			if (generatorOptions.InputAssemblies == null)
			{
				return Error.InputAssemblyFailedLoad;
			}

			if (!Path.IsPathRooted(generatorOptions.OutputAssemblyPath))
			{
				generatorOptions.OutputAssemblyPath = Path.GetFullPath(generatorOptions.OutputAssemblyPath);
			}

			try
			{
				_proxyGenerator.Generate(generatorOptions);
			}
			catch (Exception exc)
			{
				error.WriteLine(exc.Message);
				return Error.Unknown;				
			}

			return Error.None;
		}

		public static void Main(string[] args)
		{
			Program program = new Program();
			int exitCode = program.Execute(Console.Error, args);
			Environment.Exit(exitCode);
		}

		public static Assembly[] LoadInputAssemblies( string[] inputAssemblyPaths, TextWriter error )
		{
			List<Assembly> inputAssemblies = new List<Assembly>();
			List<string> failedPaths = new List<string>();

			foreach (string inputAssemblyPath in inputAssemblyPaths)
			{
				try
				{
					Assembly inputAssembly = Assembly.LoadFrom(inputAssemblyPath);
					inputAssemblies.Add(inputAssembly);
				}
				catch
				{
					failedPaths.Add(inputAssemblyPath);
				}
			}

			if (failedPaths.Count > 0)
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine("Failed loading one or more InputAssemblies:");
				foreach (string failedPath in failedPaths)
				{
					builder.Append('\t');
					builder.AppendLine(failedPath);
				}
				error.WriteLine(builder.ToString());
				return null;
			}

			return inputAssemblies.ToArray();
		}

		public static IProxyGenerator CreateProxyGenerator(string generator)
		{
			string assemblyQualifiedName;

			switch (generator.ToLowerInvariant())
			{
				case "castle":
					assemblyQualifiedName = "NHibernate.ProxyGenerators.Castle.CastleProxyGenerator, NHibernate.ProxyGenerators.Castle";
					break;
				default:
					assemblyQualifiedName = generator;
					break;
			}

			try
			{
				Type proxyGeneratorType = Type.GetType(assemblyQualifiedName, false, true);
				if( proxyGeneratorType == null )
				{
					throw new ProxyGeneratorException("Invalid Generator Type '{0}'", assemblyQualifiedName);
				}

				IProxyGenerator proxyGenerator = Activator.CreateInstance(proxyGeneratorType) as IProxyGenerator;
				if( proxyGenerator == null )
				{
					throw new ProxyGeneratorException("Generator Type does not implement IProxyGenerator '{0}'", proxyGeneratorType.AssemblyQualifiedName);
				}

				return proxyGenerator;
			}
			catch(Exception exc)
			{
				throw new ProxyGeneratorException("Error Creating _proxyGenerator of type '{0}'.\n\t{1}", assemblyQualifiedName, exc.Message);
			}
		}

		public static Assembly SearchInputDirectoryForAssembly(string inputDirectory, string assemblyFileName)
		{
			string asmFileName = assemblyFileName.Split(',')[0];
			string exeFileName = Path.Combine(inputDirectory, asmFileName + ".exe");
			if (File.Exists(exeFileName))
			{
				return Assembly.LoadFile(exeFileName);
			}
			string dllFileName = Path.Combine(inputDirectory, asmFileName + ".dll");
			if (File.Exists(dllFileName))
			{
				return Assembly.LoadFile(dllFileName);
			}
			return null;
		}

		public static IEnumerable<string> GetInputDirectories(string[] inputFilePaths)
		{
			List<string> inputDirectories = new List<string>(inputFilePaths.Length);

			foreach(string inputFilePath in inputFilePaths)
			{
				string inputFileFullPath = Path.GetFullPath(inputFilePath);
				string inputDirectory = Path.GetDirectoryName(inputFileFullPath);
				if (!inputDirectories.Contains(inputDirectory))
				{
					inputDirectories.Add(inputDirectory);
				}
			}

			return inputDirectories;
		}
	}

	public static class Error
	{
		public const int None = 0;
		public const int Unknown = 1;
		public const int InvalidArguments = 2;
		public const int InputAssemblyFailedLoad = 3;
		public const int CreateProxyGenerator = 4;
		public const int InvalidGenerator = 5;
	}
}
