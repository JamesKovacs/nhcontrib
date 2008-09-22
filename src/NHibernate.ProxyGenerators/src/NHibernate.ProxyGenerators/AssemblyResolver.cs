#region license
// Copyright (c) 2005 - 2007 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

// ADAPTED FROM NHIBERNATE.QUERY.GENERATOR
#endregion

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