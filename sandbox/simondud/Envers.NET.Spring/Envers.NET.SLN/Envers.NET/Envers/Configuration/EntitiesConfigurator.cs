using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NHibernate.Envers.Configuration.Metadata;
using NHibernate.Envers.Tools.Graph;
using NHibernate.Envers.Entities;
using NHibernate.Mapping;
using NHibernate.Envers.Configuration.Metadata.Reader;
using Iesi.Collections.Generic;
using System.IO;

namespace NHibernate.Envers.Configuration
{
    /**
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class EntitiesConfigurator {
        public EntitiesConfigurations Configure(NHibernate.Cfg.Configuration cfg, 
                                                GlobalConfiguration globalCfg, AuditEntitiesConfiguration verEntCfg,
                                                XmlDocument revisionInfoXmlMapping, XmlElement revisionInfoRelationMapping) 
        {
            // Creating a name register to capture all audit entity names created.
            AuditEntityNameRegister auditEntityNameRegister = new AuditEntityNameRegister();
            //XmlWriter writer = new XmlTextWriter(..

            // Sorting the persistent class topologically - superclass always before subclass
            IEnumerator<PersistentClass> classes = GraphTopologicalSort.Sort<PersistentClass, String>(new PersistentClassGraphDefiner(cfg)).GetEnumerator();

            ClassesAuditingData classesAuditingData = new ClassesAuditingData();
            IDictionary<PersistentClass, EntityXmlMappingData> xmlMappings = new Dictionary<PersistentClass, EntityXmlMappingData>();

            // Reading metadata from annotations
            while (classes.MoveNext()) {
                PersistentClass pc = classes.Current;

                // Collecting information from annotations on the persistent class pc
                AnnotationsMetadataReader annotationsMetadataReader =
                        new AnnotationsMetadataReader(globalCfg, pc);
                ClassAuditingData auditData = annotationsMetadataReader.AuditData;

                classesAuditingData.AddClassAuditingData(pc, auditData);
            }

            // Now that all information is read we can update the calculated fields.
            classesAuditingData.UpdateCalculatedFields();

            AuditMetadataGenerator auditMetaGen = new AuditMetadataGenerator(cfg, globalCfg, verEntCfg,
                    revisionInfoRelationMapping, auditEntityNameRegister, classesAuditingData);

            // First pass
            foreach (KeyValuePair<PersistentClass, ClassAuditingData> pcDatasEntry in classesAuditingData.GetAllClassAuditedData())
            {
                PersistentClass pc = pcDatasEntry.Key;
                ClassAuditingData auditData = pcDatasEntry.Value;

                EntityXmlMappingData xmlMappingData = new EntityXmlMappingData();
                if (auditData.IsAudited()) {
                    if (!String.IsNullOrEmpty(auditData.AuditTable.value)){ //  .getAuditTable().value())) {
                        verEntCfg.AddCustomAuditTableName(pc.EntityName, auditData.AuditTable.value);
                    }

                    auditMetaGen.GenerateFirstPass(pc, auditData, xmlMappingData, true);
			    } else {
				    auditMetaGen.GenerateFirstPass(pc, auditData, xmlMappingData, false);
			    }

                xmlMappings.Add(pc, xmlMappingData);
            }

            // Second pass
            foreach (KeyValuePair<PersistentClass, ClassAuditingData> pcDatasEntry in classesAuditingData.GetAllClassAuditedData())
            {
                EntityXmlMappingData xmlMappingData = xmlMappings[pcDatasEntry.Key];

                if (pcDatasEntry.Value.IsAudited()) {
                    auditMetaGen.GenerateSecondPass(pcDatasEntry.Key, pcDatasEntry.Value, xmlMappingData);
                    try {
                        //cfg.AddDocument(writer.write(xmlMappingData.MainXmlMapping));
                        cfg.AddDocument(xmlMappingData.MainXmlMapping);
                        //WriteDocument(xmlMappingData.getMainXmlMapping());

                        foreach (XmlDocument additionalMapping in  xmlMappingData.AdditionalXmlMappings) {
                            //cfg.AddDocument(writer.write(additionalMapping));
                            cfg.AddDocument(additionalMapping);
                            //WriteDocument(additionalMapping);
                        }
                    }
                    catch (MappingException e)
                    { //catch (DocumentException e) {  //?Catalina DocumentException NOT IMPLEMENTED
                        throw new MappingException(e);
                    }
                }
            }

            // Only if there are any versioned classes
            if (classesAuditingData.GetAllClassAuditedData().Count > 0)
            {
                try {
                    if (revisionInfoXmlMapping !=  null) {
                        //WriteDocument(revisionInfoXmlMapping);
                        //cfg.addDocument(writer.write(revisionInfoXmlMapping));
                        cfg.AddDocument((revisionInfoXmlMapping));
                    }
                }
                catch (MappingException e)
                { //catch (DocumentException e) { //?Catalina
                    throw new MappingException(e);
                }
            }

		    return new EntitiesConfigurations(auditMetaGen.EntitiesConfigurations,
				    auditMetaGen.NotAuditedEntitiesConfigurations);
        }

        //@SuppressWarnings({"UnusedDeclaration"})
        private void WriteDocument(XmlDocument e) {
            ////Todo in second implementation phase
            ////java: ByteArrayOutputStream baos = new ByteArrayOutputStream();
            ////java: Writer w = new PrintWriter(baos);
            ////byte[] baos = new byte[0];
            ////XmlTextWriter w = new XmlTextWriter( //  XmlTextWriter((TextWriter)baos);
            //try {
            //    //java: XMLWriter xw = new XMLWriter(w, new OutputFormat(" ", true));
            //    XmlTextWriter xw = new XmlTextWriter(baos., Encoding.GetEncodings().DefaultIfEmpty<null>));// .GetEncoding(System.Threading.Thread.CurrentThread.CurrentCulture.
            //    XmlDocument xw = new XmlDocument();
            //    XmlDocument
            //    w.flush();
            //} catch (IOException e1) {
            //    e1.printStackTrace();
            //}

            //Console.WriteLine("-----------");
            //Console.WriteLine(baos.toString());
            //Console.WriteLine("-----------");
        }
    }
}
