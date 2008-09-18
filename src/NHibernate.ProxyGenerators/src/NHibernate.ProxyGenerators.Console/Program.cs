namespace NHibernate.ProxyGenerators.Console
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Text;
	using Castle;

	public class Program
	{
		public static void Main(string[] args)
		{
			IProxyGenerator generator = new CastleProxyGenerator();

			ProxyGeneratorOptions generatorOptions = generator.GetOptions();
			if (Parser.ParseArgumentsWithUsage(args, generatorOptions) == false)
			{
				Environment.Exit(Error.InvalidArguments);
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

			List<Assembly> inputAssemblies = new List<Assembly>();
			foreach(string inputAssemblyPath in generatorOptions.InputAssemblyPaths)
			{
				List<string> failedPaths = new List<string>();

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
				
				if( failedPaths.Count > 0 )
				{
					StringBuilder builder = new StringBuilder();
					builder.AppendLine("Failed loading one or more InputAssemblies:");
					foreach (string failedPath in failedPaths)
					{
						builder.Append('\t');
						builder.AppendLine(failedPath);
					}
					Console.Error.WriteLine(builder.ToString());
					Environment.Exit(Error.InputAssemblyFailedLoad);
				}
			}

			string outputAssemblyPath = generatorOptions.OutputAssemblyPath;
			if( !Path.IsPathRooted(outputAssemblyPath) )
			{
				outputAssemblyPath = Path.GetFullPath(outputAssemblyPath);
			}

			try
			{
				generator.Generate(outputAssemblyPath, inputAssemblies.ToArray());
			}
			catch(Exception exc)
			{
				Console.Error.WriteLine(exc.Message);
				Environment.Exit(Error.Unknown);
				return;
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
	}
}
