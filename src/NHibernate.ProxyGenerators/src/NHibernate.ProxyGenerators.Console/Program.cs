namespace NHibernate.ProxyGenerators.Console
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Text;

	public class Program
	{
		public static void Main(string[] args)
		{
			ProxyGeneratorOptions generatorOptions = new ProxyGeneratorOptions();
			if (Parser.ParseArgumentsWithUsage(args, generatorOptions) == false)
			{
				Environment.Exit(Error.InvalidArguments);
				return;
			}

			IProxyGenerator generator;
			try
			{
				generator = CreateProxyGenerator(generatorOptions.Generator);
			}
			catch(Exception exc)
			{
				Console.Error.WriteLine(exc.Message);
				Environment.Exit(Error.CreateProxyGenerator);
				return;
			}

			generatorOptions = generator.GetOptions();
			if (Parser.ParseArgumentsWithUsage(args, generatorOptions) == false)
			{
				Environment.Exit(Error.InvalidArguments);
				return;
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

			generatorOptions.InputAssemblies = LoadInputAssemblies(generatorOptions.InputAssemblyPaths);
			if (generatorOptions.InputAssemblies == null)
			{
				Environment.Exit(Error.InputAssemblyFailedLoad);
				return;
			}

			if( !Path.IsPathRooted(generatorOptions.OutputAssemblyPath) )
			{
				generatorOptions.OutputAssemblyPath = Path.GetFullPath(generatorOptions.OutputAssemblyPath);
			}

			try
			{
				generator.Generate(generatorOptions);
			}
			catch(Exception exc)
			{
				Console.Error.WriteLine(exc.Message);
				Environment.Exit(Error.Unknown);
				return;
			}
		}

		public static Assembly[] LoadInputAssemblies( string[] inputAssemblyPaths )
		{
			List<Assembly> inputAssemblies = new List<Assembly>();
			List<string> failedPaths = new List<string>();

			foreach (string inputAssemblyPath in inputAssemblyPaths)
			{
				string inputAssemblyFullPath = Path.GetFullPath(inputAssemblyPath);
				try
				{
					Assembly inputAssembly = Assembly.LoadFile(inputAssemblyFullPath);
					inputAssemblies.Add(inputAssembly);
				}
				catch
				{
					failedPaths.Add(inputAssemblyFullPath);
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
				Console.Error.WriteLine(builder.ToString());
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
				throw new ProxyGeneratorException("Error Creating ProxyGenerator of type '{0}'.\n\t{1}", assemblyQualifiedName, exc.Message);
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
		public const int Unknown = 1;
		public const int InvalidArguments = 2;
		public const int InputAssemblyFailedLoad = 3;
		public const int CreateProxyGenerator = 4;
	}
}
