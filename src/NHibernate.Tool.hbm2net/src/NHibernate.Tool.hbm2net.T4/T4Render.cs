using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TextTemplating;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Xml;
using log4net;
using System.Reflection;

namespace NHibernate.Tool.hbm2net.T4
{
    /// <summary>
    /// A render using T4 templates
    /// </summary>
    public class T4Render:AbstractRenderer,ICanProvideStream
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        string template;
       
        string directoryForOutput=string.Empty;
        const string T4DefaultTemplate = "res://NHibernate.Tool.hbm2net.T4.templates.hbm2net.tt";
        const string DefaultTemplateForFileName = "clazz.GeneratedName+\".generated.cs\"";
        string templateForFileName;
        
        public override void Render(string savedToPackage, string savedToClass, ClassMapping classMapping, IDictionary class2classmap, StreamWriter writer)
        {
            Engine T4; 
            CallContext.LogicalSetData("clazz", classMapping);
            CallContext.LogicalSetData("savedToClass", savedToClass);
            CallContext.LogicalSetData("class2classmap", class2classmap);
            T4 = new Engine();
            string res = T4.ProcessTemplate(CustomHost.GetTemplateCode(template)
                                            , new CustomHost() 
                                            { 
                                                TemplateFile=template 
                                                ,Logger=log
                                            }
                                            );
            log.Debug("Generated File:\n"+res);
            writer.Write(res);
            writer.Flush();
            if (writer.BaseStream is FileStream)
            {
                log.Info("Flushed file:"+(writer.BaseStream as FileStream).Name);
            }
        }

        private string GetTemplateForOutputName()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<#@ property name=\"clazz\" type=\"ClassMapping\" processor=\"PropertyProcessor\" #>");
            sb.AppendLine("<#=" + templateForFileName.Trim() + " #>");
            return sb.ToString();
        }
        public override void Configure(DirectoryInfo workingDirectory, NameValueCollection props)
        {
            template = props["template"] ?? T4DefaultTemplate;
            template = template.Trim();
            templateForFileName = props["output"] ?? DefaultTemplateForFileName;
            base.Configure(workingDirectory, props);
        }
        public int MyProperty { get; set; }
        #region ICanProvideStream Members

        public Stream GetStream(ClassMapping clazz,string directory,out string fileName)
        {
            directoryForOutput = directory;
            if (!string.IsNullOrEmpty(directoryForOutput))
            {
                if (!Directory.Exists(directoryForOutput))
                    Directory.CreateDirectory(directoryForOutput);
            }

            try
            {
                var T4 = new Engine();
                var host = new CustomHost()
                {
                    TemplateFile = "config"
                    ,
                    Logger = log
                };
                CallContext.LogicalSetData("clazz", clazz);
                fileName = T4.ProcessTemplate(GetTemplateForOutputName()
                                            , host
                                            ).Trim();
                if (host.HasError)
                    throw new Exception(); // errors are logged by the engine

            }
            catch
            {
                fileName = clazz.GeneratedName + ".cs";
                log.Error("Error processing template for file name. " + fileName + " used.");
            }

            string toSave = Path.Combine(directoryForOutput, fileName);

            FileStream output = new FileStream(toSave, FileMode.Create, FileAccess.ReadWrite);
            log.Debug("Flushing output on file:" + output.Name);
            fileName = toSave;
            return output;
        }

        #endregion
    }
}
