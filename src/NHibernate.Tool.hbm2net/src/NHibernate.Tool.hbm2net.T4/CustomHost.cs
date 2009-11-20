using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using System.Reflection;
using System.IO;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml;
using log4net;

namespace NHibernate.Tool.hbm2net.T4
{
    public class CustomHost : ITextTemplatingEngineHost
    {
        static Dictionary<string, Type> directiveProcessors = new Dictionary<string, Type>();
        public bool HasError { get; internal set; }
        public IList<CompilerError> Errors { get; internal set; }
        public ILog Logger { get; set; }

        public CustomHost()
        {
            Errors = new List<CompilerError>();            
        }
        static CustomHost()
        {
            InitializeProcessors();
        }
        private static void InitializeProcessors()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeof(DirectiveProcessor).IsAssignableFrom(t))
                {
                    directiveProcessors[t.Name] = t;
                }
            }
        }
        #region ITextTemplatingEngineHost Members

        public object GetHostOption(string optionName)
        {
            switch (optionName)
            {
                case "CacheAssemblies":
                    return true;

            }
            return null;
        }
        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            try
            {
                content = GetTemplateCode(requestFileName);
                location = requestFileName;
                return true;
            }
            catch
            {
                content = location = null;
                return false;
            }
        }

        public void LogErrors(CompilerErrorCollection errors)
        {
            if (errors.HasErrors)
            {
                HasError = true;
                foreach (CompilerError ce in errors)
                {
                    Errors.Add(ce);
                    if (ce.IsWarning)
                        Logger.Warn(ce.ErrorText);
                    else
                        Logger.Error(ce.ErrorText);
                }
            }
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            HasError = false;
            // this can cause memory leaks. But since hbm2net is generally used
            // by a one-stot application, this should be acceptable.
            // creating a new appdomain would be the desired solution, but at present
            // ClassMapping is not serializable, and I dont want to change the hbm2net internals...
            return AppDomain.CurrentDomain;

        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (File.Exists(assemblyReference))
            {
                return assemblyReference;
            }
            string candidate = Path.Combine(Path.GetDirectoryName(this.TemplateFile), assemblyReference);
            if (File.Exists(candidate))
            {
                return candidate;
            }
            return "";
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            if (directiveProcessors.ContainsKey(processorName))
                return directiveProcessors[processorName];
            throw new Exception(string.Format("Directive {0} not found", processorName));
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            throw new NotImplementedException();
        }

        public string ResolvePath(string path)
        {
            return path;
        }

        public void SetFileExtension(string extension)
        {
            
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            
        }

        public IList<string> StandardAssemblyReferences
        {
            get
            {
                return new string[]
                {
                    typeof(System.Uri).Assembly.Location
                    ,GetType().Assembly.Location
                    ,typeof(ClassMapping).Assembly.Location
                    ,typeof(XmlElement).Assembly.Location
                    ,typeof(IQueryable).Assembly.Location
                };
            }
        }

        public IList<string> StandardImports
        {
            get 
            {
                return new string[]
                {
                    "System"
                    ,"System.IO"
                    ,"System.Collections"
                    ,"System.Collections.Generic"
                    ,"System.Linq"
                    ,"System.Text"
                    ,"System.Xml"
                    ,"NHibernate.Tool.hbm2net"
                    
                };
            }
 
        }

        public string TemplateFile
        {
            get;
            set;
        }

        #endregion

        public static string GetTemplateCode(string template)
        {
            Stream templateStream;
            if (template.StartsWith("res://"))
            {
                templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(template.Substring(6).Trim());
            }
            else
            {
                templateStream = new FileStream(template, FileMode.Open);
            }
            string code = new StreamReader(templateStream).ReadToEnd();
            templateStream.Close();
            return code;
        }
    }
}
