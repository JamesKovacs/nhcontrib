using System.Collections.Generic;
using NHibernate.Annotations;
using NHibernate.Annotations.Cfg.Annotations;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Mapping;
using NHibernate.Util;

namespace NHibernate.Cfg
{
	/// <summary>
	/// Enable a proper set of the FK columns in respect with the id column order
	/// Allow the correct implementation of the default EJB3 values which needs both
	/// sides of the association to be resolved
	/// </summary>
	public class ToOneFkSecondPass : FkSecondPass
	{
		private bool unique;
		private ExtendedMappings mappings;
		private string path;
		private string entityClassName;

		public ToOneFkSecondPass(ToOne value,
		                         Ejb3JoinColumn[] columns,
		                         bool unique,
		                         string entityClassName,
		                         string path,
		                         ExtendedMappings mappings) : base(value, columns)
		{
			this.mappings = mappings;
			this.unique = unique;
			this.entityClassName = entityClassName;
			this.path = entityClassName != null ? path.Substring(entityClassName.Length + 1) : path;
		}


		public override string ReferencedEntityName
		{
			get { return ((ToOne) value).ReferencedEntityName; }
		}

		public override bool IsInPrimaryKey
		{
			get
			{
				if (entityClassName == null) return false;
				/* readonly*/
				PersistentClass persistentClass = mappings.GetClass(entityClassName);
				Property property = persistentClass.IdentifierProperty;
				if (path == null)
					return false;
				else if (property != null)
				{
					//try explicit identifier property
					return path.StartsWith(property.Name + ".");
				}
				else
				{
					//try the embedded property
					//embedded property starts their path with 'id.' See PropertyPreloadedData( ) use when idClass != null in AnnotationBinder
					if (path.StartsWith("id."))
					{
						IKeyValue valueIdentifier = persistentClass.Identifier;
						string localPath = path.Substring(3);
						if (valueIdentifier is Component)
						{
							var it = ((Component) valueIdentifier).PropertyIterator.GetEnumerator();
							while (it.MoveNext())
							{
								Property idProperty = (Property) it.Current;
								if (localPath.StartsWith(idProperty.Name)) return true;
							}
						}
					}
				}
				return false;
			}
		}

		public override void DoSecondPass(IDictionary<string, PersistentClass> persistentClasses)
		{
			if (value is ManyToOne)
			{
				ManyToOne manyToOne = (ManyToOne) value;
				PersistentClass @ref;
				persistentClasses.TryGetValue(manyToOne.ReferencedEntityName,out @ref);
				if (@ref == null)
				{
					throw new AnnotationException(
						"@OneToOne or @ManyToOne on "
						+ StringHelper.Qualify(entityClassName, path)
						+ " references an unknown entity: "
						+ manyToOne.ReferencedEntityName);
				}
				BinderHelper.CreateSyntheticPropertyReference(columns, @ref, null, manyToOne, false, mappings);
				TableBinder.BindFk(@ref, null, columns, manyToOne, unique, mappings);

				//HbmBinder does this only when property-ref != null, but IMO, it makes sense event if it is null
				if (!manyToOne.IsIgnoreNotFound) manyToOne.CreatePropertyRefConstraints(persistentClasses);
			}
			else if (value is OneToOne)
			{
				((OneToOne) value).CreateForeignKey();
			}
			else
			{
				throw new AssertionFailure("FkSecondPass for a wrong value type: " + value.GetType().Name);
			}
		}
	}
}