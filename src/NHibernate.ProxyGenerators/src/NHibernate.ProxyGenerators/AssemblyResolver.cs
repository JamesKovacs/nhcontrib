namespace NHibernate.ProxyGenerators
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Text;

	[Serializable]
	public class AssemblyResolver : IDisposable
	{
		private readonly IEnumerable<string> _inputDirectories;

		public AssemblyResolver(IEnumerable<string> inputFilePaths)
		{
			_inputDirectories = GetInputDirectories(inputFilePaths);
			AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
		}

		public virtual Assembly[] LoadFrom(IEnumerable<string> assemblyPaths)
		{
			List<Assembly> assemblies = new List<Assembly>();
			List<string> failedPaths = new List<string>();

			foreach (string assemblyPath in assemblyPaths)
			{
				try
				{
					Assembly inputAssembly = Assembly.LoadFrom(assemblyPath);
					assemblies.Add(inputAssembly);
				}
				catch
				{
					failedPaths.Add(assemblyPath);
				}
			}

			if (failedPaths.Count > 0)
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendLine("Failed loading one or more Assemblies:");
				foreach (string failedPath in failedPaths)
				{
					builder.Append('\t');
					builder.AppendLine(failedPath);
				}
				throw new ProxyGeneratorException(builder.ToString());
			}

			return assemblies.ToArray();
		}

		protected virtual Assembly AssemblyResolve(object sender, ResolveEventArgs e)
		{
			foreach (string inputDirectory in _inputDirectories)
			{
				Assembly inputAssembly = SearchInputDirectoryForAssembly(inputDirectory, e.Name);

				if (inputAssembly != null) return inputAssembly;
			}

			return null;
		}

		protected static Assembly SearchInputDirectoryForAssembly(string inputDirectory, string assemblyFileName)
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

		protected static IEnumerable<string> GetInputDirectories(IEnumerable<string> inputFilePaths)
		{
			List<string> inputDirectories = new List<string>();

			foreach (string inputFilePath in inputFilePaths)
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

		public virtual void Dispose()
		{
			AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
		}
	}
}