using System;
using System.Reflection;
using log4net;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Annotations.Cfg.Annotations
{
	public class PropertyBinder
	{
		private ILog log = LogManager.GetLogger(typeof (PropertyBinder));
		private string name;
		private string returnedClassName;
		private bool lazy;
		private String propertyAccessorName;
		private Ejb3Column[] columns;
		private PropertyHolder holder;
		private ExtendedMappings mappings;
		private IValue value;
		private bool insertable = true;
		private bool updatable = true;
		private String cascade;

		/// <summary>
		/// property can be null
		/// prefer propertyName to property.getName() since some are overloaded
		/// </summary>
		private PropertyInfo property;

		private System.Type returnedClass;

		public void SetInsertable(bool insertable)
		{
			this.insertable = insertable;
		}

		public void SetUpdatable(bool updatable)
		{
			this.updatable = updatable;
		}


		public void SetName(string name)
		{
			this.name = name;
		}

		public void SetReturnedClassName(string returnedClassName)
		{
			this.returnedClassName = returnedClassName;
		}

		public void SetLazy(bool lazy)
		{
			this.lazy = lazy;
		}

		public void SetPropertyAccessorName(String propertyAccessorName)
		{
			this.propertyAccessorName = propertyAccessorName;
		}

		public void SetColumns(Ejb3Column[] columns)
		{
			insertable = columns[0].IsInsertable;
			updatable = columns[0].IsUpdatable;
			//concsistency is checked later when we know the proeprty name
			this.columns = columns;
		}

		public void SetHolder(PropertyHolder holder)
		{
			this.holder = holder;
		}

		public void SetValue(IValue value)
		{
			this.value = value;
		}

		public void SetCascade(String cascadeStrategy)
		{
			cascade = cascadeStrategy;
		}

		public void SetMappings(ExtendedMappings mappings)
		{
			this.mappings = mappings;
		}

		private void ValidateBind()
		{
			//TODO check necessary params for a bind		
		}

		private void ValidateMake()
		{
			//TODO check necessary params for a make
		}

		public void SetProperty(PropertyInfo property)
		{
			this.property = property;
		}

		public void SetReturnedClass(System.Type returnedClass)
		{
			this.returnedClass = returnedClass;
		}

		public Property Bind()
		{
			ValidateBind();
			log.DebugFormat("binding property {0} with lazy={1}", name, lazy);
			string containerClassName = holder == null ? null : holder.ClassName;
			var value = new SimpleValueBinder();
			value.SetMappings(mappings);
			value.SetPropertyName(name);
			value.SetReturnedClassName(returnedClassName);
			value.SetColumns(columns);
			value.SetPersistentClassName(containerClassName);
			value.SetType(property, returnedClass);
			value.SetMappings(mappings);
			SimpleValue propertyValue = value.Make();
			SetValue(propertyValue);
			Property prop = Make();
			holder.AddProperty(prop, columns);
			return prop;
		}

		private Property Make()
		{
			throw new NotImplementedException();
		}
	}
}