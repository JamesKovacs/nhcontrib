using System.IO;
using System.Xml;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.ByteCode.LinFu;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System;
using NUnit.Framework;
using Iesi.Collections;
using System.Collections.Generic;

namespace NHibernate.Tool.hbm2net.Tests
{
	/// <summary>
	/// Summary description for TestHelper.
	/// </summary>
	internal class TestHelper
	{
        class FileObserver:IFileCreationObserver
        {
            List<string> paths = new List<string>();
            #region IFileCreationObserver Members

            public void FileCreated(string path)
            {
                paths.Add(path);
            }

            #endregion
            public string[] Files { get { return paths.ToArray(); } }
        }
        public const string T4Renderer = "NHibernate.Tool.hbm2net.T4.T4Render, NHibernate.Tool.hbm2net";
        public const string T4DefaultTemplate = "res://NHibernate.Tool.hbm2net.templates.hbm2net.tt";
		public static DirectoryInfo DefaultOutputDirectory
		{
			get { return new DirectoryInfo("generated"); }
		}
        public static FileInfo CreateConfigFile(FileInfo configFile, string templateFile, string renderer, string package)
        {
            return CreateConfigFile(configFile, templateFile, renderer, package, null);
        }
		public static FileInfo CreateConfigFile(FileInfo configFile, string templateFile, string renderer, string package,string output)
		{
			XmlDocument xmlDoc = CreateConfigXml(templateFile, renderer, package,output);
			xmlDoc.Save(configFile.FullName);
			return configFile;
		}

		public static FileInfo CreateConfigFile(string templateFile, string renderer, string package)
		{
			return CreateConfigFile(new FileInfo(Path.GetTempFileName()), templateFile, renderer, package);
		}

        public static Assembly BuildAssemblyFromHbm(string assemblyName,params string[] mappings)
        {
            //uses hbm2net to create classes files.
            FileInfo configFile = new FileInfo(Path.GetTempFileName());
            List<string> hbms = new List<string>();
            // the mapping file needs to be written to the same 
            // directory as the config file for this test	
            foreach (var hbm in mappings)
            {
                FileInfo mappingFile = new FileInfo(Path.Combine(configFile.DirectoryName, hbm));
                if (mappingFile.Exists)
                    mappingFile.Delete();
                ResourceHelper.WriteToFileFromResource(mappingFile, hbm);
                hbms.Add(mappingFile.FullName);
            }
            TestHelper.CreateConfigFile(configFile, T4DefaultTemplate, T4Renderer, "unused", "clazz.GeneratedName+\".generated.cs\"");
            List<string> args = new List<string>();
            args.Add("--config=" + configFile.FullName);
            args.AddRange(hbms);
            FileObserver fo = new FileObserver();
            CodeGenerator.Generate(args.ToArray(), fo);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters options = new CompilerParameters();
            options.GenerateInMemory = true;
            options.ReferencedAssemblies.Add(typeof(ISet).Assembly.Location);
            options.OutputAssembly = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName + ".dll");
            options.GenerateInMemory = false;
            CompilerResults res = provider.CompileAssemblyFromFile(options, fo.Files);
            foreach (var e in res.Errors)
                Console.WriteLine("Compiler Error:" + e);
            Assert.AreEqual(0, res.Errors.Count);
            return res.CompiledAssembly;
        }

        /// <summary>
        /// Obtain a working configuration with SQLite
        /// from: http://ayende.com/Blog/archive/2009/04/28/nhibernate-unit-testing.aspx
        /// </summary>
        /// <returns>An nh configuration</returns>
        public static Configuration GetAWorkingConfiguration()
        {
            return new Configuration()
                                    .SetProperty(NHibernate.Cfg.Environment.ReleaseConnections, "on_close")
                                    .SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName)
                                    .SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName)
                                    .SetProperty(NHibernate.Cfg.Environment.ConnectionString, "data source=:memory:")
                                    .SetProperty(NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, typeof(ProxyFactoryFactory).AssemblyQualifiedName)
                                    ;
        }
        /// <summary>
        /// Obtain a working configuration for sql express
        /// </summary>
        /// <returns></returns>
        public static Configuration GetASqlExpressConfiguration()
        {
            try
            {
                return new Configuration()
                                        .SetProperty(NHibernate.Cfg.Environment.ReleaseConnections, "on_close")
                                        .SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(MsSql2005Dialect).AssemblyQualifiedName)
                                        .SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, typeof(SqlClientDriver).AssemblyQualifiedName)
                                        .SetProperty(NHibernate.Cfg.Environment.ConnectionString, @"Server=localhost\SQLEXPRESS;initial catalog=db2hbm;Integrated Security=True")
                                        .SetProperty(NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, typeof(ProxyFactoryFactory).AssemblyQualifiedName)
                                        ;
            }
            catch (Exception e)
            {
                throw new Exception("This test requires SQLExpress installed, with a db named db2hbm created.");
            }
        }
        public static XmlDocument CreateConfigXml(string templateFile, string renderer, string package)
        {
            return CreateConfigXml(templateFile, renderer, package, null);
        }
		public static XmlDocument CreateConfigXml(string templateFile, string renderer, string package,string output)
		{
			// TODO: Look into using a serialized struct to encapsulate the config.xml
			string configXml = ResourceHelper.GetResource("config.xml");
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(configXml);
			if (package != null)
			{
                xmlDoc.SelectSingleNode("/codegen/generate").Attributes["package"].InnerText = string.Empty;
			}
			if (renderer != null)
			{
				xmlDoc.SelectSingleNode("/codegen/generate").Attributes["renderer"].Value = renderer;
			}
           
			if (templateFile != null)
			{
				//<param name="template"></param>
				XmlElement param = xmlDoc.CreateElement("param");
				XmlAttribute name = xmlDoc.CreateAttribute("name");
				name.Value = "template";
				param.Attributes.Append(name);
				param.InnerText = templateFile;
				xmlDoc.SelectSingleNode("/codegen/generate").AppendChild(param);
                if (null != output)
                {
                    param = xmlDoc.CreateElement("param");
                    name = xmlDoc.CreateAttribute("name");
                    name.Value = "output";
                    param.Attributes.Append(name);
                    param.InnerText = "clazz.GeneratedName+\".generated.cs\"";
                    xmlDoc.SelectSingleNode("/codegen/generate").AppendChild(param);
                }
			}
			return xmlDoc;
		}
	}
}