namespace NHibernate.ProxyGenerators.Test
{
	using System;
	using System.IO;
	using System.Reflection;
	using NUnit.Framework;
	using Proxy;
	using Test2;

	[Serializable]
	public abstract class ProxyGeneratorTester
	{
		protected IProxyGenerator _generator;
		protected string _outputAssemblyPath;

		[SetUp]
		public virtual void SetUp()
		{
			_generator = CreateGenerator();
			_outputAssemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NHibernate.ProxyGenerators.Test.Proxies.dll");
		}

		protected abstract IProxyGenerator CreateGenerator();

		protected abstract ProxyGeneratorOptions CreateOptions(string outputAssemblyPath, params Assembly[] inputAssembiles);

		[Test]
		public void Generates_Proxies_From_Single_Assembly()
		{
			CrossAppDomainCaller.RunInOtherAppDomain(delegate
			{
				Type personType = typeof(Person);
				Assembly proxyAssembly = _generator.Generate(CreateOptions(_outputAssemblyPath, personType.Assembly));

				Assert.IsNotNull(proxyAssembly);

				Type personProxyType = null;
				foreach (Type type in proxyAssembly.GetTypes())
				{
					if (type.BaseType == personType)
					{
						personProxyType = type;
						break;
					}
				}


				Assert.IsNotNull(personProxyType);

				Assert.IsTrue(typeof(INHibernateProxy).IsAssignableFrom(personProxyType));
			});
		}

		[Test]
		public void Generates_Proxies_From_Multiple_Assemblies()
		{
			CrossAppDomainCaller.RunInOtherAppDomain(delegate
			{
				Type personType = typeof(Person);
				Type addressType = typeof(Address);
				Assembly proxyAssembly = _generator.Generate(CreateOptions(_outputAssemblyPath, personType.Assembly, addressType.Assembly));

				Assert.IsNotNull(proxyAssembly);

				Type personProxyType = null;
				Type addressProxyType = null;
				foreach (Type type in proxyAssembly.GetTypes())
				{
					if (type.BaseType == personType)
					{
						personProxyType = type;
					}
					else if (type.BaseType == addressType)
					{
						addressProxyType = type;
					}
				}

				Assert.IsNotNull(personProxyType);
				Assert.IsNotNull(addressProxyType);

				Assert.IsTrue(typeof(INHibernateProxy).IsAssignableFrom(personProxyType));
				Assert.IsTrue(typeof(INHibernateProxy).IsAssignableFrom(addressProxyType));
			});
		}

		[Test]
		public void Entity_Can_Reside_In_Assembly_Separate_From_Mapping()
		{
			CrossAppDomainCaller.RunInOtherAppDomain(delegate
			{
				Type userType = typeof(User);

				Assembly entityAssembly = userType.Assembly;
				Assembly mappingAssembly = typeof(Person).Assembly;

				Assert.AreNotEqual(entityAssembly, mappingAssembly);

				Assembly proxyAssembly = _generator.Generate(CreateOptions(_outputAssemblyPath, mappingAssembly));

				Assert.IsNotNull(proxyAssembly);

				Type userProxyType = null;
				foreach (Type type in proxyAssembly.GetTypes())
				{
					if (type.BaseType == userType)
					{
						userProxyType = type;
						break;
					}
				}

				Assert.IsNotNull(userProxyType);

				Assert.IsTrue(typeof(INHibernateProxy).IsAssignableFrom(userProxyType));
			});
		}

		[TearDown]
		public virtual void TearDown()
		{
			if( File.Exists(_outputAssemblyPath) )
			{
				File.Delete(_outputAssemblyPath);
			}
		}
	}
}