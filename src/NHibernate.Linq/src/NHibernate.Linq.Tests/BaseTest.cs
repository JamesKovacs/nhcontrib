using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NHibernate.Linq.Tests.Entities;
using NUnit.Framework;

namespace NHibernate.Linq.Tests
{
	public class BaseTest
	{
		protected NorthwindContext db;
		protected TestContext nhib;
		protected NorthwindContext nwnd;
		protected ISession session;

		protected virtual string ConnectionStringName
		{
			get { return "Northwind"; }
		}

		static BaseTest()
		{
			new GlobalSetup().SetupNHibernate();
		}

		[SetUp]
		public virtual void Setup()
		{
			session = CreateSession();
			nwnd = db = new NorthwindContext(session);
			nhib = new TestContext(session);
		}

		protected virtual ISession CreateSession()
		{
			IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString);
			con.Open();
			return GlobalSetup.CreateSession(con);
		}

		[TearDown]
		public virtual void TearDown()
		{
			session.Connection.Dispose();
			session.Dispose();
			session = null;
		}
	}
}