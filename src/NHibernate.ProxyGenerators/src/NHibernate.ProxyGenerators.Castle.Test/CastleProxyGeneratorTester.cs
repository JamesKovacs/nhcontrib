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
			_generator.Generate("OutputAssembly.dll");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InputAssemblies_Cannot_Be_Null()
		{
			_generator.Generate("C:\\Test.dll", null);
		}

		[Test]
		[ExpectedException(typeof(ProxyGeneratorException))]
		public void At_Least_One_InputAssembly_Is_Required()
		{
			_generator.Generate("C:\\Test.dll", new Assembly[0]);
		}

		[Test]
		public void At_Least_One_ClassMapping_Is_Required()
		{
			Assembly inputAssembly = typeof(string).Assembly;
			string inputAssemblyLocation = inputAssembly.Location;

			ProxyGeneratorException exc = null;
			try
			{
				_generator.Generate("C:\\Test.dll", inputAssembly);	
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