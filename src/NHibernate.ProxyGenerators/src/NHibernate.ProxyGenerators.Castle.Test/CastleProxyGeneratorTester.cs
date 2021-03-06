namespace NHibernate.ProxyGenerators.Castle.Test
{
	using System;
	using System.Reflection;
	using ProxyGenerators.Test;
	using NUnit.Framework;

	[TestFixture]
	[Serializable]
	public class CastleProxyGeneratorTester : ProxyGeneratorTester
	{
		protected override IProxyGenerator CreateGenerator()
		{
			return new CastleProxyGenerator();
		}

		protected override ProxyGeneratorOptions CreateOptions(string outputAssemblyPath, params string[] inputAssembilyPaths)
		{
			return new CastleProxyGeneratorOptions(outputAssemblyPath, inputAssembilyPaths);
		}

		[Test]
		[ExpectedException(typeof(ProxyGeneratorException))]
		public void OutputAssemblyPath_Is_Required()
		{
			_generator.Generate(null);
		}

		[Test]
		[ExpectedException(typeof(ProxyGeneratorException))]
		public void OutputAssemblyPath_Must_Be_Rooted()
		{
			_generator.Generate(CreateOptions("OutputAssembly.dll"));
		}

		[Test]
		[ExpectedException(typeof(ProxyGeneratorException))]
		public void InputAssemblies_Cannot_Be_Null()
		{
			_generator.Generate(CreateOptions("C:\\Test.dll", null));
		}

		[Test]
		[ExpectedException(typeof(ProxyGeneratorException))]
		public void At_Least_One_InputAssembly_Is_Required()
		{
			_generator.Generate(CreateOptions("C:\\Test.dll", new string[0]));
		}

		[Test]
		public void At_Least_One_ClassMapping_Is_Required()
		{
			string inputAssemblyLocation = typeof(string).Assembly.Location;

			ProxyGeneratorException exc = null;
			try
			{
				_generator.Generate(CreateOptions("C:\\Test.dll", inputAssemblyLocation));	
			}
			catch(Exception e)
			{
				exc = e as ProxyGeneratorException;
			}

			Assert.IsNotNull(exc);
			Assert.Less(0, exc.Message.IndexOf(inputAssemblyLocation));
		}
	}
}