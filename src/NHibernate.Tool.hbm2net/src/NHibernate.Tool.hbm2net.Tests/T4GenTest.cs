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

        const string T4Renderer = "NHibernate.Tool.hbm2net.T4.T4Render, NHibernate.Tool.hbm2net.T4";
        const string T4DefaultTemplate = "res://NHibernate.Tool.hbm2net.T4.templates.hbm2net.tt";
        private const string MappingFileResourceName = "Simple1.hbm.xml";
        private const string ExpectedFileResourceName = "Simple1.csharp";
        private static string ExpectedFileName = Path.Combine(TestHelper.DefaultOutputDirectory.FullName, @"Simple.generated.cs");


        /*
        [Test]
        public void TestWithADomainModel()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());
            List<string> domainMappings = new List<string>();	
            foreach (var hbmFile in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (Regex.IsMatch(hbmFile, @".*NHDomainModel\..*\.hbm\.xml"))
                {
                    FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbmFile));
                    if (mappingFile.Exists)
                        mappingFile.Delete();
                    ResourceHelper.WriteToFileFromResource(mappingFile, hbmFile.Replace("NHibernate.Tool.hbm2net.Tests.",""));
                    domainMappings.Add(mappingFile.FullName);
                }
            }

            TestHelper.CreateConfigFile(configFile, T4DefaultTemplate, T4Renderer, "unused");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);

            List<string> args = new List<string>() { "--config=" + configFile.FullName };
            args.AddRange(domainMappings);
            CodeGenerator.Generate(args.ToArray());
            Assert.Fail("useless test");
        }
        */
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

            TestHelper.CreateConfigFile(configFile, T4DefaultTemplate, T4Renderer, "unused","clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args,this);
            //this is just cheating...
            AssertFile();
            //this is better...
            Assembly asm = AssertedCompileGeneratedFiles("SimpleAssembly");
            CheckMappingAgainstCode(asm,mappingFile.FullName);
            
        }
        [Test]
        public void TestPet()
        {

            FileInfo configFile = new FileInfo(Path.GetTempFileName());

            // the mapping file needs to be written to the same 
            // directory as the config file for this test			
            string hbm = "pet.hbm.xml";
            FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
            if (mappingFile.Exists)
                mappingFile.Delete();
            ResourceHelper.WriteToFileFromResource(mappingFile, hbm);

            TestHelper.CreateConfigFile(configFile, T4DefaultTemplate, T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

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
        [Ignore]
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

            TestHelper.CreateConfigFile(configFile, T4DefaultTemplate, T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("QuickStart");
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

            TestHelper.CreateConfigFile(configFile, T4DefaultTemplate, T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

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

            TestHelper.CreateConfigFile(configFile, T4DefaultTemplate, T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

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

            TestHelper.CreateConfigFile(configFile, T4DefaultTemplate, T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

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

            TestHelper.CreateConfigFile(configFile, T4DefaultTemplate, T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");

            // ensure that test is setup correctly
            Assert.IsTrue(configFile.Exists && configFile.Length != 0);
            Assert.IsTrue(mappingFile.Exists && mappingFile.Length != 0);
            Assert.AreEqual(mappingFile.DirectoryName, configFile.DirectoryName);

            string[] args = new string[] { "--config=" + configFile.FullName, mappingFile.FullName };
            CodeGenerator.Generate(args, this);


            Assembly asm = AssertedCompileGeneratedFiles("Eg");
            CheckMappingAgainstCode(asm, mappingFile.FullName);

        }
        /// <summary>
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
        private void CheckMappingAgainstCode(Assembly asm, string mappingFile)
        {
            Configuration cfg = GetAWorkingConfiguration();
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
