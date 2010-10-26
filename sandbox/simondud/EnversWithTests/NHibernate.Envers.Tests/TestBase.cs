using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NHibernate.Envers.Tests
{
    public abstract class TestBase
    {
        public ISession Session{ get; private set; }
        public IAuditReader AuditReader { get; private set; }
        private Cfg.Configuration cfg;

        [SetUp]
        public void BaseSetup()
        {
            cfg = new Cfg.Configuration();
            cfg.Configure();
            addMappings();
        	cfg.IntegrateWithEnvers();
            var sf = cfg.BuildSessionFactory();
            Session = sf.OpenSession();
            AuditReader = AuditReaderFactory.Get(Session);
            createDropSchema(true);
        }



        private void createDropSchema(bool both)
        {
            var se = new SchemaExport(cfg);
            se.Drop(false, true);
            if(both)
                se.Create(false,true);
        }

        [TearDown]
        public void BaseTearDown()
        {
            Session.Close();
            createDropSchema(false);
        }

        protected abstract IEnumerable<string> Mappings { get; }

        protected virtual string MappingAssembly
        {
            get { return "NHibernate.Envers.Tests"; }
        }

        private void addMappings()
        {
            var ass = Assembly.Load(MappingAssembly);
            foreach (var mapping in Mappings)
            {
                cfg.AddResource(MappingAssembly + "." + mapping, ass);
            }
        }
    }
}