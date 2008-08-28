using System;
using System.Persistence;
using System.Reflection;
using log4net;
using NHibernate.Cfg;
using NHibernate.Mapping;
using NHibernate.Util;

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
			//consistency is checked later when we know the proeprty name
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
			ValidateMake();
			log.Debug("Building property " + name);
			Property prop = new Property();
			prop.Name = name;
			prop.NodeName = name;
			prop.Value = value;
			prop.IsLazy = lazy;
			prop.Cascade = cascade;
			prop.PropertyAccessorName = propertyAccessorName;
			GeneratedAttribute ann = property != null ? AttributeHelper.GetFirst<GeneratedAttribute>(property) : null;
			GenerationTime? generated = ann != null ? ann.Value : null;

			if (generated != null)
			{
				if (!GenerationTime.Never.Equals(generated))
				{
					if (AttributeHelper.IsAttributePresent<VersionAttribute>(property) && GenerationTime.Insert.Equals(generated))
					{
						throw new AnnotationException("[Generated(Insert)] on a [Version] property not allowed, use ALWAYS: " +
						                              StringHelper.Qualify(holder.Path, name));
					}
					insertable = false;
					if (GenerationTime.Always.Equals(generated))
					{
						updatable = false;
					}
					prop.Generation = GenerationTimeConverter.Convert(generated.Value);
				}
			}
			//TODO: Natural Id port.
			//NaturalIdAttribute naturalId = property != null ? AttributeHelper.GetFirst<NaturalIdAttribute>(property) : null;
			//if (naturalId != null)
			//{
			//    if (!naturalId.IsMutable)
			//    {
			//        updatable = false;
			//    }
			//    prop.setNaturalIdentifier(true);
			//}

			prop.IsInsertable = insertable;
			prop.IsUpdateable = updatable;
			var lockAnn = property != null ? AttributeHelper.GetFirst<OptimisticLockAttribute>(property) : null;
			if (lockAnn != null)
			{
				prop.IsOptimisticLocked = !lockAnn.IsExcluded;
				//TODO this should go to the core as a mapping validation checking
				if (lockAnn.IsExcluded &&
				    (AttributeHelper.IsAttributePresent<VersionAttribute>(property) ||
				     AttributeHelper.IsAttributePresent<IdAttribute>(property)
				     || AttributeHelper.IsAttributePresent<EmbeddedIdAttribute>(property)))
				{
					throw new AnnotationException(
						"[OptimisticLock].IsExclude==true incompatible with [Id], [EmbeddedId] and [Version]: " +
						StringHelper.Qualify(holder.Path, name));
				}
			}
			log.Info("Cascading " + name + " with " + cascade);
			return prop;
		}
	}
}