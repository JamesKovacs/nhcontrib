using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Annotations.Cfg.Annotations
{
	public class SimpleValueBinder
	{
		private ILog log = LogManager.GetLogger(typeof (SimpleValueBinder));
		private string propertyName;
		private string returnedClassName;
		private Ejb3Column[] columns;
		private string persistentClassName;
		private string explicitType = string.Empty;
		private IDictionary<string, string> typeParameters = new Dictionary<string, string>();
		private ExtendedMappings mappings;
		private Table table;

		public void SetPropertyName(string propertyName)
		{
			this.propertyName = propertyName;
		}

		public void SetReturnedClassName(string returnedClassName)
		{
			this.returnedClassName = returnedClassName;
		}

		public void SetTable(Table table)
		{
			this.table = table;
		}

		public void SetColumns(Ejb3Column[] columns)
		{
			this.columns = columns;
		}


		public void SetPersistentClassName(string persistentClassName)
		{
			this.persistentClassName = persistentClassName;
		}


		public void SetExplicitType(string explicitType)
		{
			this.explicitType = explicitType;
		}

		//FIXME raise an assertion failure  if setExplicitType(String) and setExplicitType(Type) are use at the same time
		public void SetExplicitType(TypeAttribute typeAnn)
		{
			if (typeAnn != null)
			{
				explicitType = typeAnn.Type;
				typeParameters.Clear();
				foreach (ParameterAttribute param in typeAnn.Parameters)
				{
					typeParameters.Add( param.Name, param.Value);
				}
			}
		}

		public void SetMappings(ExtendedMappings mappings)
		{
			this.mappings = mappings;
		}

		private void Validate()
		{
			//TODO check necessary params
			Ejb3Column.checkPropertyConsistency(columns, propertyName);
		}

		public SimpleValue Make()
		{
			Validate();
			log.DebugFormat("building SimpleValue for {0}", propertyName);
			if (table == null)
			{
				table = columns[0].Table;
			}
			return FillSimpleValue(new SimpleValue(table));
		}

		public SimpleValue FillSimpleValue(SimpleValue simpleValue)
		{
			string type = BinderHelper.IsDefault(explicitType) ? returnedClassName : explicitType;
			TypeDef typeDef = mappings.GetTypeDef(type);
			if (typeDef != null)
			{
				type = typeDef.TypeClass;
				simpleValue.TypeParameters = typeDef.Parameters;
			}
			if (typeParameters != null && typeParameters.Count != 0)
			{
				//explicit type params takes precedence over type def params
				simpleValue.TypeParameters = typeParameters;
			}
			simpleValue.TypeName = type;
			if (persistentClassName != null)
			{
				simpleValue.SetTypeUsingReflection(persistentClassName, propertyName,"");
			}
			foreach (Ejb3Column column in columns)
			{
				column.linkWithValue(simpleValue);
			}
			return simpleValue;
		}

		public void SetType(PropertyInfo property, System.Type returnedClass )
		{
			throw new NotImplementedException();
		}
	}
}