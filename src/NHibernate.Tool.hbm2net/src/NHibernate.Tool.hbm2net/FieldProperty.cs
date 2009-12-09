using System;
using System.Reflection;

using log4net;

using MultiMap = System.Collections.Hashtable;
using Element = System.Xml.XmlElement;
using System.Collections.Generic;

namespace NHibernate.Tool.hbm2net
{
	public class FieldProperty : MappingElement
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private List<ClassName> genericArguments = new List<ClassName>();
        
		public string fieldcase
		{
			get
			{
				//if (fieldName.Substring(0, 1) == fieldName.Substring(0, 1).ToLower())
				//	return "_" + fieldName;
				return fieldName.Substring(0, 1).ToLower() + fieldName.Substring(1);
			}
		}
        public void AddGenericArgument(ClassName generic)
        {
            genericArguments.Add(generic);
        }
        public ClassName[] GenericArguments { get { return genericArguments.ToArray();} }
		public string Propcase
		{
			get { return fieldName; }
		}

		public virtual string FieldName
		{
			get { return this.fieldName; }
		}

		public virtual string AccessorName
		{
			get { return this.accessorName; }
		}

		private string GetterType
		{
			get { return (FullyQualifiedTypeName.Equals("boolean")) ? "is" : "get"; }
		}

		public virtual string FullyQualifiedTypeName
		{
			get { return classType.FullyQualifiedName; }
		}

		public virtual bool Identifier
		{
			get { return id; }
		}

		public virtual bool Nullable
		{
			get { return nullable; }
		}

		public virtual bool Generated
		{
			get { return generated; }
		}

		/// <summary> Returns the classType. </summary>
		/// <returns> ClassName
		/// </returns>
		public virtual ClassName ClassType
		{
			get { return classType; }
		}

		private ClassName Type
		{
			set { this.classType = value; }
		}

		/// <summary> Returns the foreignClass.</summary>
		/// <returns> ClassName
		/// </returns>
		/// <summary> Sets the foreignClass.</summary>
		public virtual ClassName ForeignClass
		{
			get { return foreignClass; }

			set { this.foreignClass = value; }
		}

		/// <summary> Returns the foreignKeys.</summary>
		/// <returns> Set
		/// </returns>
		public virtual SupportClass.SetSupport ForeignKeys
		{
			get { return foreignKeys; }
		}

		/// <summary> Method getGetterSignature.</summary>
		/// <returns> String
		/// </returns>
		public virtual string GetterSignature
		{
			get { return GetterType + AccessorName + "()"; }
		}

		public virtual bool GeneratedAsProperty
		{
			get { return GetMetaAsBool("gen-property", true); }
		}

		/// <summary> </summary>
		/// <returns>  Return the implementation specific type for this property. e.g. java.util.ArrayList when the type is java.util.List;
		/// </returns>
		public virtual ClassName ImplementationClassName
		{
			get { return implementationClassName; }
		}

		/// <returns>
		/// </returns>
		public virtual ClassMapping ParentClass
		{
			get { return parentClass; }
		}

		public virtual string FieldScope
		{
			get { return GetScope("scope-field", "private"); }
		}

		public virtual string PropertyGetScope
		{
			get { return GetScope("scope-get", "public"); }
		}

		public virtual string PropertySetScope
		{
			get { return GetScope("scope-set", "public"); }
		}

		/// <summary>the field name </summary>
		private string fieldName = null;

		/// <summary>the property name </summary>
		private string accessorName = null;

		/// <summary>true if this is part of an id </summary>
		private bool id = false;


		private bool generated = false;
		private bool nullable = true;
		private ClassName classType;
		private ClassName foreignClass;
		private SupportClass.SetSupport foreignKeys;
		private ClassName implementationClassName;
		private ClassMapping parentClass = null;

		public FieldProperty(Element element, MappingElement parent, string name, ClassName type, bool nullable,
		                     MultiMap metaattribs) : base(element, parent)
		{
			InitWith(name, type, type, nullable, id, false, null, null, metaattribs);
		}

		public FieldProperty(Element element, MappingElement parent, string name, ClassName type, bool nullable, bool id,
		                     bool generated, MultiMap metaattribs) : base(element, parent)
		{
			InitWith(name, type, type, nullable, id, generated, null, null, metaattribs);
		}

		public FieldProperty(Element element, MappingElement parent, string name, ClassName type,
		                     ClassName implementationClassName, bool nullable, ClassName foreignClass,
		                     SupportClass.SetSupport foreignKeys, MultiMap metaattribs) : base(element, parent)
		{
			InitWith(name, type, implementationClassName, nullable, id, false, foreignClass, foreignKeys, metaattribs);
		}
        public bool IsValueType { get; private set; }
		protected internal virtual void InitWith(string name, ClassName type, ClassName implementationClassName, bool nullable,
		                                         bool id, bool generated, ClassName foreignClass,
		                                         SupportClass.SetSupport foreignKeys, MultiMap metaattribs)
		{
			this.fieldName = name.Replace("`","");
			Type = type;
			this.nullable = nullable;
			this.id = id;
			this.generated = generated;
			this.implementationClassName = implementationClassName;
			this.accessorName = BeanCapitalize(fieldName);
			this.foreignClass = foreignClass;
			this.foreignKeys = foreignKeys;
			MetaAttribs = metaattribs;
			if (fieldName.Substring(0, 1) == fieldName.Substring(0, 1).ToLower())
				log.Warn("Nonstandard naming convention found on " + fieldName);
            IsValueType = CheckValueType(type);
		}

        private bool CheckValueType(ClassName type)
        {
            string name = type.FullyQualifiedName;
            if (0 > name.IndexOf("."))
                name = "System." + name;
            var t = System.Type.GetType(name);
            if (null != t)
                return t.IsValueType;
            return false;
        }

        

		/// <summary> foo -> Foo
		/// FOo -> FOo
		/// 
		/// </summary>
		/// <returns>
		/// </returns>
		private string BeanCapitalize(string fieldname)
		{
			if ((Object) fieldname == null || fieldname.Length == 0)
			{
				return fieldname;
			}

			if (fieldname.Length > 1 && Char.IsUpper(fieldname[1]) && Char.IsUpper(fieldname[0]))
			{
				return fieldname;
			}
			char[] chars = fieldname.ToCharArray();
			chars[0] = Char.ToUpper(chars[0]);
			return new String(chars);
		}

		public override string ToString()
		{
			return FullyQualifiedTypeName + ":" + FieldName;
		}

		public virtual string GetScope(string localScopeName, string defaultScope)
		{
			if ((Object) defaultScope == null)
				defaultScope = "private";
			return (GetMeta(localScopeName) == null) ? defaultScope : GetMetaAsString(localScopeName);
		}
	}
}