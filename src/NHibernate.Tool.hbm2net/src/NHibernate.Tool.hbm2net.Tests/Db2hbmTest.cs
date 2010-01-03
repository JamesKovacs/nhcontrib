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
using XmlUnit;

namespace NHibernate.Tool.hbm2net.Tests
{
    [TestFixture]
    public class Db2hbmTest:IStreamProvider
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
            string hbm = "JustProperties.hbm.xml";
            TestHelper.BuildAssemblyFromHbm("NHibernate.DomainModel", hbm);
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmConfigTemplate.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Simple"));
            CheckXmlMapping(hbm,"Simple");
        }

        private void CheckXmlMapping(string hbm,string stream)
        {
            XmlDiff diff = new XmlDiff(new XmlInput(internalStreams[stream].ToString())
                                       , new XmlInput(ResourceHelper.GetResource(hbm)));

            var res = diff.Compare();
            if (!res.Equal)
            {
                Console.WriteLine("Expected xml was:");
                Console.WriteLine(ResourceHelper.GetResource(hbm));
                Console.WriteLine("But was:");
                Console.WriteLine(internalStreams[stream].ToString());
            }
            Assert.IsTrue(res.Equal);
        }

        private string GetSchemaForSQLite(params string[] streams)
        {
            var cfg = TestHelper.GetAWorkingConfiguration();
            foreach (string s in streams)
                cfg.AddXmlString(ResourceHelper.GetResource(s));
            SchemaExport se = new SchemaExport(cfg);
            StringBuilder sb = new StringBuilder();
            se.Execute((string q) => sb.AppendLine(q), true, false);
            return sb.ToString();
        }
        private string GetSchemaForMSSql(params string[] streams)
        {
            var cfg = TestHelper.GetASqlExpressConfiguration();
            foreach (string s in streams)
                cfg.AddXmlString(ResourceHelper.GetResource(s));
            SchemaExport se = new SchemaExport(cfg);
            StringBuilder sb = new StringBuilder();
            se.Execute((string q) => sb.AppendLine(q), true, false);
            return sb.ToString();
        }


        #region IStreamProvider Members
        Dictionary<string, StringBuilder> internalStreams = new Dictionary<string,StringBuilder>();
        public TextWriter GetTextWriter(string entityName )
        {
            internalStreams[entityName] = new StringBuilder();
            return new StringWriter(internalStreams[entityName]);
        }

        #endregion
    }
}
