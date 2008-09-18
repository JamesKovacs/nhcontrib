namespace NHibernate.ProxyGenerators.Castle.Test
{
	using System;
	using System.Collections;
	using global::Castle.DynamicProxy;
	using Iesi.Collections.Generic;
	using NUnit.Framework;
	using Rhino.Mocks;

	[TestFixture]
	public class CastleProxyFactoryTester
	{
		private IProxyBuilder _builder;
		private IDictionary _proxies;
		private CastleProxyFactory _generator;
		private MockRepository _mocks;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();

			_proxies = new Hashtable();
			_builder = _mocks.DynamicMock<IProxyBuilder>();
			_generator = new CastleProxyFactory(_builder, _proxies);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Depends_On_IProxyBuilder()
		{
			new CastleProxyFactory(null, new Hashtable());
		}

		[Test]
		public void Does_Not_Proxy_Generic_Entities()
		{
			_mocks.ReplayAll();

			_generator.PostInstantiate(null, typeof(GenericEntity<>), null, null, null, null);

			_mocks.VerifyAll();

			Assert.AreEqual(0, _proxies.Count);
		}

		[Test]
		public void Generates_Class_Proxy_When_Only_One_Interface()
		{
			Type entityType = typeof(Entity);
			HashedSet<Type> interfaces = new HashedSet<Type>();
			interfaces.Add(typeof(IEntityInterceptor1));

			SetupResult.For(_builder.CreateClassProxy(entityType, ProxyGenerationOptions.Default))
				.Return(null)
				.Repeat.Once();

			_mocks.ReplayAll();

			_generator.PostInstantiate(string.Empty, typeof(Entity), interfaces, null, null, null);

			_mocks.VerifyAll();
		}

		[Test]
		public void Generates_Interface_Proxy_When_More_Than_One_Interface()
		{
			HashedSet<Type> interfaces = new HashedSet<Type>();

			interfaces.Add(typeof(IEntityInterceptor1));
			interfaces.Add(typeof(IEntityInterceptor2));

			SetupResult.For(_builder.CreateInterfaceProxyTypeWithoutTarget(null, null, ProxyGenerationOptions.Default))
				.IgnoreArguments()
				.Return(null)
				.Repeat.Once();

			_mocks.ReplayAll();

			_generator.PostInstantiate(string.Empty, typeof(Entity), interfaces, null, null, null);

			_mocks.VerifyAll();
		}

		class Entity
		{
		}

		interface IEntityInterceptor1
		{
		}

		interface IEntityInterceptor2
		{
		}

		class GenericEntity<T>
		{
		}
	}
}