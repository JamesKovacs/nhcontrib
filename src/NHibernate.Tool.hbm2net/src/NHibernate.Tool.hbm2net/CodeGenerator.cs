using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Linq;
using log4net;
using log4net.Config;

using MultiHashMap = System.Collections.Hashtable;
using MultiMap = System.Collections.Hashtable;
using Document = System.Xml.XmlDocument;
using Element = System.Xml.XmlElement;
using NHibernate.Tool.hbm2net.T4;
using System.Collections.Specialized;

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
        static Hbm2NetParameters cmdLine;
		public static void Generate(String[] args,IFileCreationObserver fileCreationObserver)
		{
            //this has to change... dirty things during porting
            ClassMapping.ResetComponents();
            nsmgr = new XmlNamespaceManager(new NameTable());
			nsmgr.AddNamespace("urn", "urn:nhibernate-mapping-2.2");
			children = new ArrayList();
			allMaps = new MultiMap();
			ArrayList mappingFiles = new ArrayList();
			string outputDir = null;
			SupportClass.ListCollectionSupport generators = new SupportClass.ListCollectionSupport();
			MultiMap globalMetas = new MultiHashMap();
			// parse command line parameters
            cmdLine = new Hbm2NetParameters(args);
            try
            {
                cmdLine.Parse();
                if (0 == cmdLine.FileNames.Count())
                {
                    Console.Error.WriteLine("No input file(s) specified");
                    throw new NotEnougthParametersException();
                }
            }
            catch (NotEnougthParametersException)
            {
                Console.Error.WriteLine(string.Format("Use:hbm2net {0} files.hbm.xml ( wildcards allowed )",cmdLine.GetShortHelp()));
                Console.Error.WriteLine(cmdLine.GetHelp());
                Environment.Exit(-1);
            }
            if (!string.IsNullOrEmpty(cmdLine.ConfigFile))
            {
                if (File.Exists(cmdLine.ConfigFile))
                {
                    FileInfo configFile = new FileInfo(cmdLine.ConfigFile);
                    // parse config xml file
                    Document document = new XmlDocument();
                    document.Load(configFile.FullName);
                    globalMetas = MetaAttributeHelper.LoadAndMergeMetaMap((document["codegen"]), null);
                    IEnumerator generateElements = document["codegen"].SelectNodes("generate").GetEnumerator();
                    while (generateElements.MoveNext())
                    {
                        generators.Add(new Generator(configFile.Directory, (Element)generateElements.Current));
                    }
                }
                else
                {
                    log.Error("Configuration file:" + cmdLine.ConfigFile + " does not exist");
                    Environment.Exit(-1);
                }
            }
            if (generators.Count == 0)
            {
                log.Info("No configuration file specified: using T4 generator with default template.");
                T4Render t4 = new T4Render();
                t4.Configure(new DirectoryInfo(Directory.GetCurrentDirectory()), new NameValueCollection());
                generators.Add(new Generator(t4));
            }
            if (!string.IsNullOrEmpty(cmdLine.OutputDir))
            {
                outputDir = cmdLine.OutputDir;
            }
            foreach (string inFile in cmdLine.FileNames)
            {
                if (inFile.IndexOf("*") > -1)
                {
                    mappingFiles.AddRange(GetFiles(inFile));
                }
                else
                {
                    mappingFiles.Add(inFile);
                }
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
                g.BaseDirName = outputDir ?? ".\\";
                g.Generate(classMappings,fileCreationObserver);
            }
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