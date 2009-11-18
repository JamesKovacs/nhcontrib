using System.IO;
using System.Xml;

namespace NHibernate.Tool.hbm2net.Tests
{
	/// <summary>
	/// Summary description for TestHelper.
	/// </summary>
	internal class TestHelper
	{
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
//			XmlDocument xmlDoc = CreateConfigXml(templateFile, renderer, package);
//			FileInfo configFile = new FileInfo(Path.GetTempFileName());
//			xmlDoc.Save(configFile.FullName);
//			return  configFile;
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
                xmlDoc.SelectSingleNode("/codegen/generate").Attributes["package"].InnerText = "";
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