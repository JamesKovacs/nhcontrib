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
using System.Data.SqlClient;
using System.Data;

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
            CleanupDb();
            internalStreams.Clear();
        }

        /// <summary>
        /// Removes all dirty FK and tables so export schema can create a new schema without pain even
        /// if test reuses same tables names, this works only on MSSQL
        /// </summary>
        private static void CleanupDb()
        {
            var cfg = TestHelper.GetASqlExpressConfiguration();
            using (SqlConnection conn = new SqlConnection(cfg.Properties[NHibernate.Cfg.Environment.ConnectionString]))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"select distinct RC.CONSTRAINT_NAME fkname ,KCU.TABLE_NAME tname from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC inner join INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU
                                  on KCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME and KCU.CONSTRAINT_CATALOG=RC.CONSTRAINT_CATALOG and KCU.CONSTRAINT_SCHEMA=RC.CONSTRAINT_SCHEMA";
                cmd.CommandType = CommandType.Text;
                var rd = cmd.ExecuteReader();
                List<string> drops = new List<string>();
                while (rd.Read())
                {
                    drops.Add(string.Format("alter table {0} drop constraint {1}",rd["tname"],rd["fkname"]));
                   
                }
                rd.Close();
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select t.TABLE_SCHEMA sch,t.TABLE_NAME name from INFORMATION_SCHEMA.TABLES t";
                rd = cmd.ExecuteReader();
                while(rd.Read())
                {
                    drops.Add(string.Format("drop table {0}.{1}", rd["sch"], rd["name"]));
                }
                rd.Close();
                foreach (var drop in drops)
                {
                    var cdel = conn.CreateCommand();
                    cdel.CommandType = CommandType.Text;
                    cdel.CommandText = drop;
                    cdel.ExecuteScalar();
                }
                
            }
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
        public void JustProperties()
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
        public void ManyToOnes()
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
        [Test]
        public void CompositeManyToOnes()
        {

            string hbm = "PropertiesAndCompositeManyToOne.hbm.xml";
            TestHelper.BuildAssemblyFromHbm("NHibernate.DomainModel3", hbm);
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTest3.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Widget"));
            Assert.IsTrue(internalStreams.ContainsKey("Producer"));
            CheckXmlMapping(hbm, "Widget", "Producer");
        }
        [Test]
        [Explicit("Integration test: AdventureWorks")]
        public void AdventureWorksIntegrationTest()
        {
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTestAdventureWorks.xml")));
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            foreach (var stream in internalStreams)
            {
                Console.WriteLine(stream.Key + ":");
                Console.WriteLine(stream.Value);
            }
        }
        [Test]
        [Explicit("Not found anyway to discrimate automatically a bag from a set")]
        public void BagCollection()
        {

            string hbm = "PropertiesAndBag.hbm.xml";
            TestHelper.BuildAssemblyFromHbm("NHibernate.DomainModel8", hbm);
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTest8.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Simple"));
            Assert.IsTrue(internalStreams.ContainsKey("Item"));
            CheckXmlMapping(hbm, "Simple", "Item");
        }
        [Test]
        public void ManyToMany()
        {
            string hbm = "PropertiesAndManyToMany.hbm.xml";
            TestHelper.BuildAssemblyFromHbm("NHibernate.DomainModel9", hbm);
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTest9.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Widget"));
            Assert.IsTrue(internalStreams.ContainsKey("Child"));
            CheckXmlMapping(hbm, "Widget", "Child");
        }
        [Test]
        public void IdBagManyToMany()
        {
            string hbm = "PropertiesAndManyToManyIdBag.hbm.xml";
            TestHelper.BuildAssemblyFromHbm("NHibernate.DomainModel10", hbm);
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTest10.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Widget"));
            Assert.IsTrue(internalStreams.ContainsKey("Child"));
            CheckXmlMapping(hbm, "Widget", "Child");
        }
        [Test]
        public void SetCollection()
        {

            string hbm = "PropertiesAndSetCollection.hbm.xml";
            TestHelper.BuildAssemblyFromHbm("NHibernate.DomainModel4", hbm);
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTest4.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Widget"));
            Assert.IsTrue(internalStreams.ContainsKey("Child"));
            Assert.IsFalse(internalStreams.ContainsKey("Item"));
            Assert.IsFalse(internalStreams.ContainsKey("CompositeItem"));
            CheckXmlMapping(hbm, "Widget", "Child");
        }
        [Test]
        public void CompositeKeySetCollection()
        {
            string hbm = "PropertiesAndCompositeKeySetCollection.hbm.xml";
            TestHelper.BuildAssemblyFromHbm("NHibernate.DomainModel5", hbm);
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTest5.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Widget"));
            Assert.IsFalse(internalStreams.ContainsKey("CompositeItem"));
            CheckXmlMapping(hbm, "Widget");
        }
        [Test]
        public void MapCollection()
        {
            string hbm = "PropertiesAndMapCollection.hbm.xml";
            TestHelper.BuildAssemblyFromHbm("NHibernate.DomainModel6", hbm);
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTest6.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Widget"));
            Assert.IsFalse(internalStreams.ContainsKey("Sample"));
            CheckXmlMapping(hbm, "Widget");
        }
        [Test]
        public void ComplexMapCollection()
        {
            string hbm = "PropertiesAndComplexMapCollection.hbm.xml";
            TestHelper.BuildAssemblyFromHbm("NHibernate.DomainModel7", hbm);
            MappingGenerator gen = new MappingGenerator();
            gen.Configure(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.Db2hbmTest7.xml")));
            string schema = GetSchemaForMSSql(hbm);
            Console.WriteLine("Generated Schema:");
            Console.Write(schema);
            internalStreams.Clear(); // clear all generated files...
            gen.Generate(this);
            Assert.IsTrue(internalStreams.ContainsKey("Widget"));
            Assert.IsFalse(internalStreams.ContainsKey("Sample"));
            CheckXmlMapping(hbm, "Widget");
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
            se.Execute((q) => sb.AppendLine(q), true, false);
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
