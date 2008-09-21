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
			if (Parser.ParseHelp(args))
			{
				Parser.ParseArguments(args, generatorOptions);
			}
			else if (Parser.ParseArguments(args, generatorOptions) == false)
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

			if (Parser.ParseHelp(args))
			{
				error.WriteLine(Parser.ArgumentsUsage(generatorOptions.GetType()));
				return Error.None;
			}
			if (Parser.ParseArguments(args, generatorOptions) == false)
			{
				error.WriteLine(Parser.ArgumentsUsage(generatorOptions.GetType()));
				return Error.InvalidArguments;
			}

			try
			{
				_proxyGenerator.Generate(generatorOptions);
			}
			catch (Exception exc)
			{
				error.WriteLine(exc.Message);
				error.WriteLine(exc.StackTrace);
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

		

		public static IProxyGenerator CreateProxyGenerator(string generator)
		{
			string assemblyQualifiedName;

			switch (generator.ToLowerInvariant())
			{
				case "castle":
					assemblyQualifiedName = "NHibernate.ProxyGenerators.Castle.CastleProxyGenerator, NHibernate.ProxyGenerators.Castle";
					break;
				case "activerecord":
					assemblyQualifiedName = "NHibernate.ProxyGenerators.ActiveRecord.ActiveRecordProxyGenerator, NHibernate.ProxyGenerators.ActiveRecord";
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
