using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NHibernate.Envers.Tests
{
	public abstract class TestBase
	{
		protected const string MappingAssembly = "NHibernate.Envers.Tests";
		protected Cfg.Configuration Cfg { get; private set; }
		protected ISession Session { get; set; }
		protected IAuditReader AuditReader { get; set; }


		[SetUp]
		public void BaseSetup()
		{
			Cfg = new Cfg.Configuration();
			Cfg.Configure();
			addMappings();
			Cfg.IntegrateWithEnvers();
			var sf = Cfg.BuildSessionFactory();
			createDropSchema(true);
			using (Session = openSession(sf))
			{
				Initialize();				
			}
			Session = openSession(sf);
			AuditReader = AuditReaderFactory.Get(Session);
		}

		private ISession openSession(ISessionFactory sessionFactory)
		{
			var session = sessionFactory.OpenSession();
			session.FlushMode = FlushMode;
			return session;
		}

		[TearDown]
		public void BaseTearDown()
		{
			Session.Close();
			createDropSchema(false);
		}

		protected virtual FlushMode FlushMode
		{
			get { return FlushMode.Auto; }
		}

		protected abstract void Initialize();

		private void createDropSchema(bool both)
		{
			var se = new SchemaExport(Cfg);
			se.Drop(false, true);
			if(both)
				se.Create(false,true);
		}

		protected abstract IEnumerable<string> Mappings { get; }

		private void addMappings()
		{
			var ass = Assembly.Load(MappingAssembly);
			foreach (var mapping in Mappings)
			{
				Cfg.AddResource(MappingAssembly + "." + mapping, ass);
			}
		}
	}
}