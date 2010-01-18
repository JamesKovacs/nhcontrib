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
using log4net.Config;

namespace NHibernate.Tool.hbm2net.Tests
{
    [TestFixture]
    public class Db2hbmTest:IStreamProvider
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            XmlConfigurator.Configure();
        }
        [SetUp]
        public void SetUp()
        {
            internalStreams.Clear();
        }
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
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTest1.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Simple"));
            CheckXmlMapping(hbm,"Simple");
        }
        [Test]
        public void EntityWithManyToOnes()
        {
            
            string hbm = "PropertiesAndManyToOne.hbm.xml";
            TestHelper.BuildAssemblyFromHbm("NHibernate.DomainModel2", hbm);
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTest2.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Widget"));
            Assert.IsTrue(internalStreams.ContainsKey("Producer"));
            CheckXmlMapping(hbm, "Widget","Producer");
        }

        private void CheckXmlMapping(string hbm,params string[] stream)
        {
            NameTable nt = new NameTable();
            nt.Add("urn:nhibernate-mapping-2.2");
            var nsmgr = new XmlNamespaceManager(nt);
            nsmgr.AddNamespace("urn", "urn:nhibernate-mapping-2.2");
            XmlDocument doc = new XmlDocument(nt);
            doc.PreserveWhitespace = true;
            doc.LoadXml(internalStreams[stream[0]].ToString());
            XmlNode refChild = doc.SelectSingleNode("//urn:class",nsmgr);
            for (int i = 1; i < stream.Length; ++i)
            {
                XmlDocument docChild = new XmlDocument(nt);
                docChild.PreserveWhitespace = true;
                docChild.LoadXml(internalStreams[stream[i]].ToString());
                doc.SelectSingleNode("/urn:hibernate-mapping",nsmgr).AppendChild(doc.ImportNode(docChild.SelectSingleNode("//urn:class",nsmgr),true));
            }
            DiffConfiguration dc = new DiffConfiguration("test", true, WhitespaceHandling.None, true);
            XmlDiff diff = new XmlDiff(new XmlInput(doc.OuterXml)
                                       , new XmlInput(ResourceHelper.GetResource(hbm))
                                       ,dc
                                       );
            var res = diff.Compare();
            if (!res.Equal)
            {
                Console.WriteLine("Expected xml was:");
                Console.WriteLine(ResourceHelper.GetResource(hbm));
                Console.WriteLine("But was:");
                Console.WriteLine(doc.InnerXml);
                
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
