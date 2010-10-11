using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NHibernate.Mapping;
using System.Collections;

namespace NHibernate.Envers.Configuration.Metadata
{
    /**
     * @author Simon Duduica, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */
    public class MetadataTools {
        public static XmlElement AddNativelyGeneratedId(XmlDocument doc, XmlElement parent, String name, String type) {
            XmlElement id_mapping = doc.CreateElement("id");
            parent.AppendChild(id_mapping);
            id_mapping.SetAttribute("name", name);
            id_mapping.SetAttribute("type", type);

            XmlElement generator_mapping = doc.CreateElement("generator");
            id_mapping.AppendChild(generator_mapping);
            generator_mapping.SetAttribute("class", "native");
            /*generator_mapping.SetAttribute("class", "sequence");
            generator_mapping.addElement("param").SetAttribute("name", "sequence").setText("custom");*/

            return id_mapping;
        }

        public static XmlElement AddProperty(XmlElement parent, String name, String type, bool insertable, bool key) {
            XmlElement prop_mapping;
            if (key) 
            {
                prop_mapping = parent.OwnerDocument.CreateElement("key-property");
            } 
            else 
            {
                prop_mapping = parent.OwnerDocument.CreateElement("property");
                //rk: only insert/update attributes on property - not key-property
                prop_mapping.SetAttribute("insert", insertable ? "true" : "false");
                prop_mapping.SetAttribute("update", "false");
            }
            parent.AppendChild(prop_mapping);

            prop_mapping.SetAttribute("name", name);
            
            if (type != null) {
                prop_mapping.SetAttribute("type", type);
            }

            return prop_mapping;
        }

        private static void AddOrModifyAttribute(XmlElement parent, String name, String value) {
            parent.SetAttribute(name,value);
            //if (attribute.Length == 0) {
            //    parent.SetAttribute(name, value);
            //} else {
            //    attribute.setValue(value);
            //}
        }

        public static XmlElement AddOrModifyColumn(XmlElement parent, String name) {
            XmlElement column_mapping = (XmlElement)parent.SelectSingleNode("column");

            if (column_mapping == null) {
                return AddColumn(parent, name, -1, 0, 0, null);
            }

            if (!String.IsNullOrEmpty(name)) {
                AddOrModifyAttribute(column_mapping, "name", name);
            }

            return column_mapping;
        }
        /// <summary>
        /// Add column
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="length"> pass -1 if you don't want this attribute</param>
        /// <param name="scale"></param>
        /// <param name="precision"></param>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public static XmlElement AddColumn(XmlElement parent, String name, int length, int scale, int precision,
									    String sqlType) {
            XmlElement column_mapping = parent.OwnerDocument.CreateElement("column");
             parent.AppendChild(column_mapping);

            column_mapping.SetAttribute("name", name);
            if (length != -1) {
                column_mapping.SetAttribute("length", length.ToString());
            }
		    if (scale != 0) {
			    column_mapping.SetAttribute("scale", scale.ToString());
		    }
		    if (precision != 0) {
			    column_mapping.SetAttribute("precision", precision.ToString());
		    }
		    if (!String.IsNullOrEmpty(sqlType)) {
                column_mapping.SetAttribute("sql-type", sqlType);
            }

            return column_mapping;
        }

        private static XmlElement CreateEntityCommon(XmlDocument document, String type, AuditTableData auditTableData,
                                                  String discriminatorValue) {
            XmlElement hibernate_mapping = document.CreateElement("hibernate-mapping");
            hibernate_mapping.SetAttribute("assembly", "NHibernate.Envers");
            hibernate_mapping.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            hibernate_mapping.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //rk: changed from Configuration.MappingSchemaXMLNS. Not available in NH3
            hibernate_mapping.SetAttribute("xmlns", "urn:nhibernate-mapping-2.2"); 
            hibernate_mapping.SetAttribute("auto-import", "false");
            document.AppendChild(hibernate_mapping);

            XmlElement class_mapping = document.CreateElement(type);
            hibernate_mapping.AppendChild(class_mapping);

            if (auditTableData.getAuditEntityName() != null) {
                class_mapping.SetAttribute("entity-name", auditTableData.getAuditEntityName());
            }

            if (discriminatorValue != null) {
                class_mapping.SetAttribute("discriminator-value", discriminatorValue);
            }

            if (!String.IsNullOrEmpty(auditTableData.getAuditTableName())) {
                class_mapping.SetAttribute("table", auditTableData.getAuditTableName());
            }

            if (!String.IsNullOrEmpty(auditTableData.getSchema())) {
                class_mapping.SetAttribute("schema", auditTableData.getSchema());
            }

            if (!String.IsNullOrEmpty(auditTableData.getCatalog())) {
                class_mapping.SetAttribute("catalog", auditTableData.getCatalog());
            }

            return class_mapping;
        }

        public static XmlElement CreateEntity(XmlDocument document, AuditTableData auditTableData, String discriminatorValue) {
            return CreateEntityCommon(document, "class", auditTableData, discriminatorValue);
        }

        public static XmlElement CreateSubclassEntity(XmlDocument document, String subclassType, AuditTableData auditTableData,
                                                   String extendsEntityName, String discriminatorValue) {
            XmlElement class_mapping = CreateEntityCommon(document, subclassType, auditTableData, discriminatorValue);

            class_mapping.SetAttribute("extends", extendsEntityName);

            return class_mapping;
        }

        public static XmlElement CreateJoin(XmlElement parent, String tableName,
                                         String schema, String catalog) {

            XmlElement join_mapping = parent.OwnerDocument.CreateElement("join");
                parent.AppendChild(join_mapping);

            join_mapping.SetAttribute("table", tableName);

            if (schema.Length != 0) {
                join_mapping.SetAttribute("schema", schema);
            }

            if (catalog.Length != 0) {
                join_mapping.SetAttribute("catalog", catalog);
            }

            return join_mapping;
        }

        /// <summary>
        /// Adds the columns in the enumerator to the any_mapping XmlElement
        /// </summary>
        /// <param name="any_mapping"></param>
        /// <param name="columns">should contain elements of Column type</param>
        public static void AddColumns(XmlElement any_mapping, IEnumerator<ISelectable> columns) {
            while (columns.MoveNext()) {
                Column column = (Column)columns.Current;
                AddColumn(any_mapping, column.Name, column.Length, column.IsPrecisionDefined()?column.Scale:0, 
                          column.IsPrecisionDefined()?column.Precision:0,column.SqlType);
            }
        }

        //TODO Simon: ORIG: @SuppressWarnings({"unchecked"})
        private static void ChangeNamesInColumnElement(XmlElement element, ColumnNameEnumerator colNameEnumerator) {
            //Iterator<XmlElement> properties = element.elementIterator();
            XmlNodeList nodeList = element.ChildNodes;
            //while (properties.hasNext()) {
            //    XmlElement property = properties.next();
            foreach (XmlElement property in nodeList){
                if ("column".Equals(property.Name)) {
                    string value = property.GetAttribute("name");
                    if (!String.IsNullOrEmpty(value)) {
                        //nameAttr.setText(columnNameIterator.next());
                        colNameEnumerator.MoveNext();
                        property.SetAttribute("name", colNameEnumerator.Current);
                    }
                }
            }
        }

        //@SuppressWarnings({"unchecked"})
        public static void PrefixNamesInPropertyElement(XmlElement element, String prefix, ColumnNameEnumerator colNameEnumerator,
                                                        bool changeToKey, bool insertable) {
            XmlNodeList nodeList = element.ChildNodes;
            foreach (XmlElement property in nodeList){
                if ("property".Equals(property.Name)) {
                    string value = property.GetAttribute("name");
                    if (!String.IsNullOrEmpty(value)) {
                        property.SetAttribute("name",prefix + value);
                    }
                    ChangeNamesInColumnElement(property, colNameEnumerator);

                    if (changeToKey) {
                        ChangeElementName(property, "key-property");
                    }

				    property.SetAttribute("insert",insertable?"true":"false");
                }
            }
        }

        /// <summary>
        /// Implements the name change of an element by creating a new element, 
        /// copying data from the former, deleting the former and attaching 
        /// the new one to the parent
        /// </summary>
        /// <param name="element"></param>
        /// <param name="newName"></param>
        private static void ChangeElementName(XmlElement element, string newName)
        {
            XmlElement newElement = element.OwnerDocument.CreateElement(newName);
            for (int i = 0; i < element.Attributes.Count; i++) {
                newElement.SetAttributeNode((XmlAttribute)element.Attributes[i].CloneNode(true));    
            }
            for (int i = 0; i < element.ChildNodes.Count; i++) {
                newElement.AppendChild(element.ChildNodes[i].CloneNode(true));
            }
            XmlNode parent = element.ParentNode;
            parent.RemoveChild(element);
            try {
                parent.AppendChild(newElement);
            }catch(Exception){
                parent.AppendChild(element);
            }
        }

        /**
         * An iterator over column names.
         */
        public abstract class ColumnNameEnumerator : IEnumerator<String>
        {
            #region IEnumerator<string> Members

            public abstract string Current
            {
                get;
            }

            #endregion

            #region IDisposable Members
            public abstract void Dispose();
            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get { throw new NotImplementedException(); }
            }
            public abstract bool MoveNext();
            public abstract void Reset();
            #endregion
        }

        public class ColumnNameEnumeratorFromEnum : ColumnNameEnumerator {
            private IEnumerator<ISelectable> _columnEnumerator;
            public ColumnNameEnumeratorFromEnum(IEnumerator<ISelectable> columnEnumerator) { _columnEnumerator = columnEnumerator; }
            public override bool MoveNext() { return _columnEnumerator.MoveNext(); }
            //TODO Simon: see if value is the intended return. 27/06/2010 - It seems not, it is the name
            public override String Current { get {return ((Column)_columnEnumerator.Current).Name;} }
            public override void Reset() { _columnEnumerator.Reset(); }
            public override void Dispose() { _columnEnumerator.Dispose(); }
        }
        public class ColumnNameEnumeratorFromArray : ColumnNameEnumerator {
            private JoinColumnAttribute[] _joinColumns;
            public ColumnNameEnumeratorFromArray(JoinColumnAttribute[] joinColumns){_joinColumns = joinColumns;}
                int counter = 0;
                public override bool MoveNext() 
                { 
                    throw new NotImplementedException();//return counter <= joinColumns < joinColumns.length; 
                }
                public override String Current 
                { 
                    get{throw new NotImplementedException();// return joinColumns[counter++].name(); 
                    }
                }
                public override void Reset() { throw new NotImplementedException(); }
                public override void Dispose() { throw new NotImplementedException(); }

        }

        public static ColumnNameEnumerator GetColumnNameEnumerator(IEnumerator<ISelectable> columnEnumerator) {
            return new ColumnNameEnumeratorFromEnum(columnEnumerator);
        }

        public static ColumnNameEnumerator GetColumnNameEnumerator(JoinColumnAttribute[] joinColumns) {
            return new ColumnNameEnumeratorFromArray(joinColumns);
        }
    }
}
