using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

using log4net;
using log4net.Config;

using MultiHashMap = System.Collections.Hashtable;
using MultiMap = System.Collections.Hashtable;
using Document = System.Xml.XmlDocument;
using Element = System.Xml.XmlElement;

namespace NHibernate.Tool.hbm2net
{
	/// <summary> </summary>
	public class CodeGenerator
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static XmlNamespaceManager nsmgr;

		private static ArrayList children;
		private static MultiMap allMaps;

        public static void Generate(String[] args)
        {
            Generate(args, null);
        }
		public static void Generate(String[] args,IFileCreationObserver fileCreationObserver)
		{
            //this has to change... dirty things during porting
            ClassMapping.ResetComponents();

            nsmgr = new XmlNamespaceManager(new NameTable());
			nsmgr.AddNamespace("urn", "urn:nhibernate-mapping-2.2");

			children = new ArrayList();
			allMaps = new MultiMap();
            
			File.Delete("error-log.txt");

			// DOMConfigurator is deprecated in the latest log4net, but we are using an earlier
			// version that comes with NVelocity.
			XmlConfigurator.Configure(new FileInfo("NHibernate.Tool.hbm2net.exe.config"));

			if (args.Length == 0)
			{
                DumpHelp();
				Environment.Exit(- 1);
			}

			ArrayList mappingFiles = new ArrayList();
			string outputDir = null;
			SupportClass.ListCollectionSupport generators = new SupportClass.ListCollectionSupport();

			MultiMap globalMetas = new MultiHashMap();
			// parse command line parameters
			for (int i = 0; i < args.Length; i++)
			{
                if (args[i].StartsWith("--"))
				{
					if (args[i].StartsWith("--config="))
					{
						FileInfo configFile = new FileInfo(args[i].Substring(9));

						// parse config xml file
						Document document = new XmlDocument();
						document.Load(configFile.FullName);
						globalMetas = MetaAttributeHelper.LoadAndMergeMetaMap((document["codegen"]), null);
						IEnumerator generateElements = document["codegen"].SelectNodes("generate").GetEnumerator();

						while (generateElements.MoveNext())
						{
							generators.Add(new Generator(configFile.Directory, (Element) generateElements.Current));
						}
					}
					else if (args[i].StartsWith("--output="))
					{
						outputDir = args[i].Substring(9);
					}
				}
				else if (args[i].IndexOf("*") > -1)
				{
					// Handle wildcards
					mappingFiles.AddRange(GetFiles(args[i]));
				}
				else
				{
					mappingFiles.Add(args[i]);
				}
			}

			// if no config xml file, add a default generator
			if (generators.Count == 0)
			{
                DumpHelp();
                Environment.Exit(-1);
			}

			Hashtable classMappings = new Hashtable();
            for (IEnumerator iter = mappingFiles.GetEnumerator(); iter.MoveNext(); )
            {
                log.Info(iter.Current.ToString());

                string mappingFile = (string)iter.Current;
                if (!Path.IsPathRooted(mappingFile))
                {
                    mappingFile = Path.Combine(Environment.CurrentDirectory, mappingFile);
                }
                if (!File.Exists(mappingFile))
                    throw new FileNotFoundException("Mapping file does not exist.", mappingFile);

                // parse the mapping file
                NameTable nt = new NameTable();
                nt.Add("urn:nhibernate-mapping-2.2");
                Document document = new XmlDocument(nt);
                document.Load(mappingFile);

                Element rootElement = document["hibernate-mapping"];

                if (rootElement == null)
                    continue;

                XmlAttribute a = rootElement.Attributes["namespace"];
                string pkg = null;
                if (a != null)
                {
                    pkg = a.Value;
                }
                MappingElement me = new MappingElement(rootElement, null);
                IEnumerator classElements = rootElement.SelectNodes("urn:class", nsmgr).GetEnumerator();
                MultiMap mm = MetaAttributeHelper.LoadAndMergeMetaMap(rootElement, globalMetas);
                HandleClass(pkg, me, classMappings, classElements, mm, false);

                classElements = rootElement.SelectNodes("urn:subclass", nsmgr).GetEnumerator();
                HandleClass(pkg, me, classMappings, classElements, mm, true);

                classElements = rootElement.SelectNodes("urn:joined-subclass", nsmgr).GetEnumerator();
                HandleClass(pkg, me, classMappings, classElements, mm, true);

                // Ok, pickup subclasses that we found before their superclasses
                ProcessChildren(classMappings);
                
            }
            // generate source files
            for (IEnumerator iterator = generators.GetEnumerator(); iterator.MoveNext(); )
            {
                Generator g = (Generator)iterator.Current;
                g.BaseDirName = outputDir;
                g.Generate(classMappings,fileCreationObserver);
            }
		}

        private static void DumpHelp()
        {
            Console.Error.WriteLine("Use: hbm2net --config=configfile.xml [--output=outdir]");
            Console.Error.WriteLine("*** Config file example: ****");
            Console.Error.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            Console.Error.WriteLine("<codegen>");
            Console.Error.WriteLine("\t<generate renderer=\"NHibernate.Tool.hbm2net.T4.T4Render, NHibernate.Tool.hbm2net.T4\" package=\"\">");
            Console.Error.WriteLine("\t\t<param name=\"template\">res://NHibernate.Tool.hbm2net.T4.templates.hbm2net.tt</param>");
            Console.Error.WriteLine("\t\t<param name=\"output\">clazz.GeneratedName+\".generated.cs\"</param>");
            Console.Error.WriteLine("\t</generate>");
            Console.Error.WriteLine("</codegen>");
            Console.Error.WriteLine("\nMultiple generation steps is achieved by multiple <codegen> nodes.");
        }

		private static ICollection GetFiles(string fileSpec)
		{
			string path = Path.GetDirectoryName(fileSpec);
			string names = Path.GetFileName(fileSpec);

			if (path == "")
			{
				path = Environment.CurrentDirectory;
			}

			DirectoryInfo di = new DirectoryInfo(path);
			FileInfo[] files = di.GetFiles(names);

			ArrayList fileNames = new ArrayList(files.Length);

			foreach (FileInfo file in files)
			{
				fileNames.Add(file.FullName);
			}

			return fileNames;
		}

		private static void HandleClass(string classPackage, MappingElement me, Hashtable classMappings,
		                                IEnumerator classElements, MultiMap mm, bool extendz)
		{
			while (classElements.MoveNext())
			{
				Element clazz = (Element) classElements.Current;

				if (!extendz)
				{
					ClassMapping cmap = new ClassMapping(classPackage, clazz, me, mm);
					SupportClass.PutElement(classMappings, cmap.FullyQualifiedName, cmap);
					SupportClass.PutElement(allMaps, cmap.FullyQualifiedName, cmap);
				}
				else
				{
					string ex = clazz.Attributes["extends"] == null ? null : clazz.Attributes["extends"].Value;
					if ((object) ex == null)
					{
						throw new MappingException("Missing extends attribute on <" + clazz.LocalName + " name=" +
						                           clazz.Attributes["name"].Value + ">");
					}

					int commaIndex = ex.IndexOf(',');
					if (commaIndex > -1)
					{
						//suppress the leading AssemblyName
						ex = ex.Substring(0, commaIndex).Trim();
					}

					ClassMapping superclass = (ClassMapping) allMaps[ex];
					if (superclass == null)
					{
						// Haven't seen the superclass yet, so record this and process at the end
						SubclassMapping orphan = new SubclassMapping(classPackage, me, ex, clazz, mm);
						children.Add(orphan);
					}
					else
					{
						ClassMapping subclassMapping = new ClassMapping(classPackage, me, superclass.ClassName, superclass, clazz, mm);
						superclass.AddSubClass(subclassMapping);
						SupportClass.PutElement(allMaps, subclassMapping.FullyQualifiedName, subclassMapping);
					}
				}
			}
		}

		/// <summary>
		/// Try to locate superclasses for any orphans we have
		/// </summary>
		private static void ProcessChildren(Hashtable classMappings)
		{
			while (FindParents(classMappings))
			{
			}

			foreach (SubclassMapping child in children)
			{
				if (child.Orphaned)
				{
					// Log that we had an orphan
					log.Warn(string.Format("Cannot extend {0} child of unmapped class {1} ", child.Name, child.SuperClass));
				}
			}
		}

		/// <summary>
		/// Find parents for any orphans
		/// </summary>
		/// <returns></returns>
		private static bool FindParents(Hashtable classMappings)
		{
			if (children.Count == 0)
			{
				// No parents to find
				return false;
			}
			else
			{
				bool found = false;

				foreach (SubclassMapping child in children)
				{
					if (child.Orphaned)
					{
						ClassMapping superclass = (ClassMapping) allMaps[child.SuperClass];
						if (superclass != null)
						{
							ClassMapping subclassMapping =
								new ClassMapping(child.ClassPackage, child.MappingElement, superclass.ClassName, superclass, child.Clazz,
								                 child.MultiMap);
							superclass.AddSubClass(subclassMapping);
							// NB Can't remove it from the iterator, so record that we've found the parent.
							child.Orphaned = false;
							found = true;
						}
					}
				}

				// Tell them if we found any
				return found;
			}
		}
	}
}