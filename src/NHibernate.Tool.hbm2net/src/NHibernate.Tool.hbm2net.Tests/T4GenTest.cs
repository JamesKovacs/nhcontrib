using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using log4net.Config;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Iesi.Collections;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Connection;
using NHibernate.ByteCode.LinFu;
using NHibernate.Tool.hbm2ddl;
using System.Xml.Schema;
using System.Xml;

namespace NHibernate.Tool.hbm2net.Tests
{
    
    [TestFixture, Category("T4 Tests")]
    public class T4GenTest:IFileCreationObserver
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            XmlConfigurator.Configure();
        }
        List<string> generatedFiles;
        [SetUp]
        public void Setup()
        {
            
            generatedFiles = new List<string>();
        }

        
        private const string MappingFileResourceName = "Simple1.hbm.xml";
        private const string ExpectedFileResourceName = "Simple1.csharp";
        private static string ExpectedFileName = Path.Combine(TestHelper.DefaultOutputDirectory.FullName, @"Simple.generated.cs");


       
        [Test]
        public void TestDefaultTemplate()
        {
            
            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			

            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, MappingFileResourceName));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, MappingFileResourceName);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName,"--output=generated" };
            CodeGenerator.Generate(args,this);
            //this is just cheating...
            AssertFile();
            //this is better...
            Assembly asm = AssertedCompileGeneratedFiles("SimpleAssembly");
            CheckMappingAgainstCode(asm,mappingFile.FullName);
            
        }
        [Test]
        public void EntityWithComponents()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "pet.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("NHibernatePets");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        [Test]
        public void UUIDCat()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "UUIDCat.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("QuickStart");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }

        [Test(Description = "mapping example from:http://ayende.com/Blog/archive/2009/06/03/nhibernate-mapping-ndash-ltmapgt.aspx")]
        public void Map()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "map.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("Map");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        
        [Test]
       
        public void KeyManyToOne()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "keymanytoone.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("KMTONE");
            try
            {
                CheckMappingAgainstCode(asm, mappingFile.FullName);
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.InnerException.Message, "composite-id class must override Equals(): Eg.Cat");
            }

        }

        [Test]
        public void Set()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "set.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("Set");
            
                CheckMappingAgainstCode(asm, mappingFile.FullName);
           

        }
        [Test]
        public void Join()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "join.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);
            Assembly asm = AssertedCompileGeneratedFiles("Join");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        [Test]
        public void DoesNotOverwriteNewerTargetIfRequired()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "product.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);

            FileInfo gen = new FileInfo(generatedFiles[0]);
            DateTime prev = gen.LastWriteTimeUtc;

            generatedFiles.Clear();
            args = new string[] { "-ct","--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);
            Assert.AreEqual(0, generatedFiles.Count);
            
            generatedFiles.Clear();
            args = new string[] {  "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);
            gen = new FileInfo(generatedFiles[0]);
            Assert.AreNotEqual(prev, gen.LastWriteTimeUtc);

        }
        [Test]
        public void GettingStarted()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "product.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("FirstSolution");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        [Test]
        public void ShouldUseDefaultConfig()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "product.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("FirstSolution");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        [Test, Explicit]
        public void InferConfigSchema()
        {
            XmlSchemaInference infer = new XmlSchemaInference();
            var schemaSet = infer.InferSchema(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.hbm2net.Tests.t4config.xml")));
            Assert.AreEqual(1, schemaSet.Schemas().Count);
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                schema.Write(Console.Out);
            }

        }
        [Test]
        public void AbstractBaseClass()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "abstractbaseaclasswithsubclass.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("AbstractBase");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        [Test]
        public void Escapes()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "escapes.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("Escapes");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        [Test(Description = "mapping example from:http://ayende.com/Blog/archive/2009/06/03/nhibernate-mapping-ndash-ltmapgt.aspx")]
        public void ComplexMap()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "complexmap.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("ComplexMap");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }

        [Test]
        public void ComplexMapWithKeyManyToOne()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "Complexmapwithkeymanytoone.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("ComplexMapKM2N");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }


        [Test(Description = "mapping example from:http://ayende.com/Blog/archive/2009/04/11/nhibernate-mapping-ltdynamic-componentgt.aspx")]
        public void DynamicComponent()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "Dynamiccomponent.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("DynamicEntity");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }

        [Test()]
        public void RawCompositeKey()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "compositekey.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("InvoiceEntities2");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        [Test()]
        public void ClassBasedCompositeKey()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "classcompositekey.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("InvoiceEntities");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        [Test(Description = "from http://ayende.com/Blog/archive/2009/04/19/nhibernate-mapping-ltone-to-onegt.aspx")]
        public void OneToOneExample()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "onetoone.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("OneToOne");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        [Test(Description="From chapter 5.1 nh doc")]
        public void Chapter5dot1Example()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "chapter5.1.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, TestHelper.T4DefaultTemplate, TestHelper.T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("Eg");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        
        private void CheckMappingAgainstCode(Assembly asm, string mappingFile)
        {
            Configuration cfg = TestHelper.GetAWorkingConfiguration();
            cfg.AddFile(new FileInfo(mappingFile));
            //cfg.AddAssembly(asm);
            //new SchemaExport(cfg).Create(true, true);
            cfg.BuildMapping();
            cfg.BuildSessionFactory();//do some sanity check on mapping and code...
        }

        private Assembly AssertedCompileGeneratedFiles(string generatedAssemblyName)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters options = new CompilerParameters();
            options.GenerateInMemory = true;
            options.ReferencedAssemblies.Add(typeof(ISet).Assembly.Location);
            options.OutputAssembly = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,generatedAssemblyName+".dll");
            options.GenerateInMemory = false;
            CompilerResults res = provider.CompileAssemblyFromFile(options,generatedFiles.ToArray());
            foreach (var e in res.Errors)
                Console.WriteLine("Compiler Error:" + e);
            Assert.AreEqual(0, res.Errors.Count);
            
            return res.CompiledAssembly;
        }
        private static void AssertFile()
        {
		    Assert.IsTrue(File.Exists(ExpectedFileName), "File not found: {0}", ExpectedFileName);
		    using (StreamReader sr = File.OpenText(ExpectedFileName))
		    {
                string s1 = ResourceHelper.GetResource(ExpectedFileResourceName);
                string s2 = sr.ReadToEnd();
                
			    Assert.AreEqual(s1.Trim(), s2.Trim());
		    }
        }

        #region IFileCreationObserver Members

        public void FileCreated(string path)
        {
            Console.WriteLine("Generated file://\"" + path+"\"");
            generatedFiles.Add(path);
        }

        #endregion
    }
}
