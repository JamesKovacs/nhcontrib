using NUnit.Framework;

namespace NHibernate.Burrow.Test.FrameworkEnvironment {
	[TestFixture]
	public class FuctionFixture  {
		private BurrowFramework f = new BurrowFramework();
		[Test]
		public void GetNHibernateCfgTest() {
			Cfg.Configuration ncfg = f.BurrowEnvironment.GetNHConfig("PersistenceUnit1");
			Assert.IsNotNull(ncfg);
		}
		
		[Test]
		public void RebuildConfiguration() {
			ISessionFactory sf = f.GetSessionFactory(typeof (MockEntities.MockEntity));
			f.BurrowEnvironment.RebuildSessionFactories();
			Assert.AreNotEqual(sf, f.GetSessionFactory(typeof(MockEntities.MockEntity)));
		}
	}
}	