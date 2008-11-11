using System;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Test.MockEntities;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConfigurationTests
{
	[TestFixture]
	public class ConfigReadWriteFixture
	{
		[Test]
		public void ConnectionStringTest()
		{
			new BurrowFramework().InitWorkSpace();
			string cs = new BurrowFramework().BurrowEnvironment.Configuration.DBConnectionString(typeof(MockEntity));
			Assert.IsTrue(cs.IndexOf("Server") >= 0);
			new BurrowFramework().CloseWorkSpace();
		}

		[Test]
		public void ReadNHConfigFileTest()
		{
			IBurrowConfig section = new BurrowFramework().BurrowEnvironment.Configuration;
			Assert.IsNotNull(section);
			Assert.IsTrue(section.PersistenceUnitCfgs.Count > 0);

			foreach (IPersistenceUnitCfg puSection in section.PersistenceUnitCfgs)
			{
				Assert.IsTrue(puSection.NHConfigFile.IndexOf(".xml") > 0);
			}
		}

		[Test]
		public void BurrowConfigruationChangeTest()
		{
			BurrowFramework f = new BurrowFramework();

			IBurrowConfig section = f.BurrowEnvironment.Configuration;

			Assert.IsNotNull(section);
			Assert.AreEqual(5, section.ConversationCleanupFrequency);

			Random r = new Random(3);
			int freq = r.Next(10, 100);
			try
			{
				section.ConversationCleanupFrequency = freq;
				Assert.Fail("Failed to throw ChangeConfigWhenRunningException");
			}
			catch (ChangeConfigWhenRunningException) { }
			f.BurrowEnvironment.ShutDown();

			section.ConversationCleanupFrequency = freq;

			Assert.AreEqual(freq, section.ConversationCleanupFrequency);
			freq = r.Next(10, 100);
			section.ConversationCleanupFrequency = freq;
			Assert.AreEqual(freq, section.ConversationCleanupFrequency);
			f.BurrowEnvironment.Start();
		}


		[Test]
		public void NHConfigruationChangeTest()
		{
 
			IFrameworkEnvironment fe = new BurrowFramework().BurrowEnvironment;
			NHibernate.Cfg.Configuration cfg = fe.GetNHConfig("PersistenceUnit1");
			cfg.SetProperty("dialect", "NHibernate.Dialect.MsSql2000Dialect");
			fe.RebuildSessionFactories();
		  	Assert.AreEqual( new BurrowFramework().GetSessionFactory(typeof (MockEntity)).Dialect.GetType(),
				typeof (NHibernate.Dialect.MsSql2000Dialect));
			cfg.SetProperty("dialect", "NHibernate.Dialect.MsSql2005Dialect");
			fe.RebuildSessionFactories();
		  	Assert.AreEqual( new BurrowFramework().GetSessionFactory(typeof (MockEntity)).Dialect.GetType(),
				typeof (NHibernate.Dialect.MsSql2005Dialect));
			 
		}
	}
}