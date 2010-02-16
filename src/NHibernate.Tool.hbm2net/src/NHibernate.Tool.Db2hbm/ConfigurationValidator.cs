using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using System.Reflection;
using System.Xml;

namespace NHibernate.Tool.Db2hbm
{
    public class ConfigurationValidator
    {
        public string WarningMessage { get; set; }
        public string ErrorMessage { get; set; }
        public ConfigurationValidator()
        {
            WarningMessage = ErrorMessage = null;
            errors = new StringBuilder();
            warnings = new StringBuilder();
        }
        StringBuilder errors;
        StringBuilder warnings;
        public void OnValidateInfo(object source, ValidationEventArgs va)
        {
            if (va.Severity == XmlSeverityType.Error)
            {
                errors.AppendLine(va.Message + FormatPosition(va.Exception));
            }
            if (va.Severity == XmlSeverityType.Warning)
            {
                warnings.AppendLine(va.Message + FormatPosition(va.Exception));
            }
        }

        private string FormatPosition(XmlSchemaException xmlSchemaException)
        {
            return string.Format(" @ line:{0} column:{1}", xmlSchemaException.LineNumber, xmlSchemaException.LinePosition);
        }

        static XmlSchemaSet schemaset;
        static ConfigurationValidator()
        {
            XmlSchema schema = XmlSchema.Read(Assembly.GetExecutingAssembly().GetManifestResourceStream("NHibernate.Tool.Db2hbm.nhibernate-codegen-2.2.xsd"), null);
            schemaset = new XmlSchemaSet();
            schemaset.Add(schema);
        }
        public void Validate(XmlDocument doc)
        {
            doc.Schemas.Add(schemaset);
            doc.Validate(new ValidationEventHandler(OnValidateInfo));
            ErrorMessage = errors.ToString();
            WarningMessage = warnings.ToString();
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                throw new ConfigurationValidationException("Configuration validation failed:\n" + ErrorMessage);
            }
        }
    }
}
