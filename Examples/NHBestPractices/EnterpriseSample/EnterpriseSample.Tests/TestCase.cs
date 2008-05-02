using System.Collections.Generic;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using EnterpriseSample.Core.DataInterfaces;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace EnterpriseSample.Tests
{
    public abstract class TestCase
    {
        //protected abstract IList<string> Assemblies { get; }
        //protected ISessionFactory sessions;

        //public TestCase()
        //{
        //    Configuration cfg = new Configuration();
        //    foreach (string assembly in Assemblies)
        //        cfg.AddAssembly(assembly);

        //    cfg.GenerateSchemaCreationScript(Dialect.GetDialect());

        //    sessions = cfg.BuildSessionFactory();
        //}

        public IDaoFactory DaoFactory
        {
            get
            {
                IWindsorContainer container = new WindsorContainer(new XmlInterpreter());
                return (IDaoFactory)container.Resolve("DaoFactory");
            }
        }
    }
}
