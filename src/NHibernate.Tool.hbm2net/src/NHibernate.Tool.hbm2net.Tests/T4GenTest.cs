using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using log4net.Config;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NHibernate.Tool.hbm2net.Tests
{
    
    [TestFixture, Category("T4 Tests")]
    public class T4GenTest
    {
        [SetUp]
        public void Setup()
        {
            XmlConfigurator.Configure();
        }

        const string T4Renderer = "NHibernate.Tool.hbm2net.T4.T4Render, NHibernate.Tool.hbm2net.T4";
        const string T4DefaultTemplate = "res://NHibernate.Tool.hbm2net.T4.templates.hbm2net.tt";
        private const string MappingFileResourceName = "Simple1.hbm.xml";
        private const string ExpectedFileResourceName = "Simple1.csharp";
        private static string ExpectedFileName = Path.Combine(TestHelper.DefaultOutputDirectory.FullName, @"Simple.generated.cs");



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
            CodeGenerator.Main(args.ToArray());

        }

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
            CodeGenerator.Main(args);
            
            
            AssertFile();
        }
        private static void AssertFile()
        {
        
		    Assert.IsTrue(File.Exists(ExpectedFileName), "File not found: {0}", ExpectedFileName);
		    using (StreamReader sr = File.OpenText(ExpectedFileName))
		    {
			    Assert.AreEqual(ResourceHelper.GetResource(ExpectedFileResourceName), sr.ReadToEnd());
		    }
        }
    }
}
