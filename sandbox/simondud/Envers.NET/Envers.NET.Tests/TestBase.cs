using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Envers;
using NHibernate.Envers.Event;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Envers.NET.Tests.NH3
{
    public abstract class TestBase
    {
        public ISession Session{ get; private set; }
        public IAuditReader AuditReader { get; private set; }
        private Configuration cfg;

        [SetUp]
        public void BaseSetup()
        {
            cfg = new Configuration();
            addListeners();
            cfg.Configure();
            addMappings();
            var sf = cfg.BuildSessionFactory();
            Session = sf.OpenSession();
            AuditReader = AuditReaderFactory.Get(Session);
            createDropSchema(true);
        }

        private void addListeners()
        {
            var listeners = new[] {new AuditEventListener()};
            cfg.AppendListeners(ListenerType.PostInsert, listeners);
            cfg.AppendListeners(ListenerType.PostUpdate, listeners);
            cfg.AppendListeners(ListenerType.PostDelete, listeners);
            cfg.AppendListeners(ListenerType.PostCollectionRecreate, listeners);
            cfg.AppendListeners(ListenerType.PreCollectionRemove, listeners);
            cfg.AppendListeners(ListenerType.PreCollectionUpdate, listeners);
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


        protected virtual IEnumerable<string> Mappings
        {
            get { return new string[0]; }
        }

        protected virtual string MappingAssembly
        {
            get { return "Envers.NET.Tests.NH3"; }
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