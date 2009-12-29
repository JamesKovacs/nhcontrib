using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Xml.Schema;
using System.Xml;
using System.Reflection;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Tool.Db2hbm;
using System.IO;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.ByteCode.LinFu;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Tool.hbm2net.Tests
{
    [TestFixture]
    public class Db2hbmTest
    {
        [Test, Explicit]
        public void InferConfigSchema()
        {
            XmlSchemaInference infer = new XmlSchemaInference();
            var schemaSet = infer.InferSchema(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmConfigTemplate.xml")));
            Assert.AreEqual(1, schemaSet.Schemas().Count);
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                schema.Write(Console.Out);
            }
        }
        [Test]
        public void CanReadConfiguration()
        {
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmConfigTemplate.xml")));
        }
        [Test]
        public void CanRunGenerator()
        {
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmConfigTemplate.xml")));
            gen.Generate(new StdoutStreamProvider());
        }
        [Test]
        public void SimpleEntityWithJustProperties()
        {
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmConfigTemplate.xml")));
            string schema = GetSchemaForSQLite("JustProperties.hbm.xml");
            gen.Generate(new StdoutStreamProvider());
            
        }

        private string GetSchemaForSQLite(params string[] streams)
        {
            var cfg = GetAWorkingConfiguration();
            foreach (string s in streams)
                cfg.AddXmlString(ResourceHelper.GetResource(s));
            SchemaExport se = new SchemaExport(cfg);
            StringBuilder sb = new StringBuilder();
            se.Execute((string q) => sb.AppendLine(q), true, false);
            return sb.ToString();
        }

        // <summary>
        /// Obtain a working configuration with SQLite
        /// from: http://ayende.com/Blog/archive/2009/04/28/nhibernate-unit-testing.aspx
        /// </summary>
        /// <returns>An nh configuration</returns>
        private Configuration GetAWorkingConfiguration()
        {
            return new Configuration()
                                    .SetProperty(NHibernate.Cfg.Environment.ReleaseConnections, "on_close")
                                    .SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName)
                                    .SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName)
                                    .SetProperty(NHibernate.Cfg.Environment.ConnectionString, "data source=:memory:")
                                    .SetProperty(NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, typeof(ProxyFactoryFactory).AssemblyQualifiedName)
                                    ;
        }
    }
}
