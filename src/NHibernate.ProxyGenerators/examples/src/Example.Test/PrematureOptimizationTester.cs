namespace Example.Test
{
	using System;
	using Domain.Models;
	using NHibernate;
	using NHibernate.Cache;
	using NHibernate.Cfg;
	using NHibernate.Dialect;
	using NUnit.Framework;
	using System.Diagnostics;

	[TestFixture]
	public class PrematureOptimizationTester
	{
		[Test]
		public void Static_Proxies_Out_Perform_Standard_Proxies()
		{
			Stopwatch sw = new Stopwatch();
			const int instances = 100;

			long time;
			long timeWithStaticProxies;

			sw.Reset();
			using(ISessionFactory factory = CreateSessionFactory())
			{
				using(ISession session = factory.OpenSession())
				{
					sw.Start();

					for (int i = 0; i < instances; i++)
					{
						session.Load<Person>(i);
					}

					sw.Stop();
					time = sw.ElapsedMilliseconds;
				}
			}

			sw.Reset();
			using (ISessionFactory factory = CreateSessionFactoryWithStaticProxies())
			{
				using (ISession session = factory.OpenSession())
				{
					sw.Start();

					for (int i = 0; i < instances; i++)
					{
						session.Load<Person>(i);
					}

					sw.Stop();
					timeWithStaticProxies = sw.ElapsedMilliseconds;
				}
			}

			Console.WriteLine("Time for {0} Instances: {1}ms", instances, time);
			Console.WriteLine("Time (static proxies) for {0} Instances: {1}ms", instances, timeWithStaticProxies);
			Assert.Less(timeWithStaticProxies, time);
		}

		private static ISessionFactory CreateSessionFactory()
		{
			return CreateSessionFactory(false);
		}

		private static ISessionFactory CreateSessionFactoryWithStaticProxies()
		{
			return CreateSessionFactory(true);
		}

		private static ISessionFactory CreateSessionFactory(bool useStaticProxies)
		{
			Configuration cfg = new Configuration();
			cfg.Properties["cache.provider_class"] = typeof(HashtableCacheProvider).AssemblyQualifiedName;
			cfg.Properties["dialect"] = typeof(MsSql2000Dialect).AssemblyQualifiedName;
			
			if (useStaticProxies)
			{
				cfg.Properties["proxyfactory.factory_class"] = "CastleStaticProxyFactoryFactory, Example.Domain.Proxies";
			}

			cfg.AddAssembly(typeof(Person).Assembly);

			return cfg.BuildSessionFactory();
		}

		
	}
}