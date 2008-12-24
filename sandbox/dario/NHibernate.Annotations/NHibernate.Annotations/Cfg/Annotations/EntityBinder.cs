using System;
using System.Collections.Generic;
using System.Persistence;
using log4net;
using NHibernate.Annotations.Extensions;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Util;

namespace NHibernate.Annotations.Cfg.Annotations
{
	public class EntityBinder
	{
		private System.Type annotatedClass;
		private int batchSize;
		private string cacheConcurrentStrategy;
		private bool cacheLazyProperty;
		private string cacheRegion;
		private string discriminatorValue = string.Empty;
		private bool dynamicInsert;
		private bool dynamicUpdate;
		private bool explicitHibernateEntityAnnotation;
		private IDictionary<String, String> filters = new Dictionary<String, String>();
		private bool ignoreIdAnnotations;
		private InheritanceState inheritanceState;
		private bool isPropertyAnnotated = false;
		private bool lazy;
		private ILog log = LogManager.GetLogger(typeof(EntityBinder));
		private ExtendedMappings mappings;
		private String name;
		private OptimisticLockType optimisticLockType;
		private PersistentClass persistentClass;
		private PolymorphismType polymorphismType;
		private String propertyAccessor;
		private System.Type proxyClass;
		private IDictionary<String, Object> secondaryTableJoins = new Dictionary<String, Object>();
		private IDictionary<String, Join> secondaryTables = new Dictionary<String, Join>();
		private bool selectBeforeUpdate;
		private String where;
		private Dialect.Dialect dialect;


		/// <summary>
		/// Use as a fake one for Collection of elements
		/// </summary>
		public EntityBinder()
		{
		}

		public EntityBinder(
			EntityAttribute hibAnn,
			System.Type annotatedClass,
			PersistentClass persistentClass,
			ExtendedMappings mappings
			)
		{
			this.mappings = mappings;
			this.persistentClass = persistentClass;
			this.annotatedClass = annotatedClass;
			BindHibernateAnnotation(hibAnn);
		}

		public bool IsAnnotatedProperty
		{
			get { return isPropertyAnnotated; }
		}

		
		private void BindHibernateAnnotation(EntityAttribute hibAnn)
		{
			if (hibAnn != null)
			{
				dynamicInsert = hibAnn.IsDynamicInsert;
				dynamicUpdate = hibAnn.IsDynamicUpdate;
				optimisticLockType = hibAnn.OptimisticLock;
				selectBeforeUpdate = hibAnn.IsSelectBeforeUpdate;
				polymorphismType = hibAnn.Polymorphism;
				explicitHibernateEntityAnnotation = true;
				//persister handled in bind
			}
			else
			{
				//default values when the annotation is not there
				dynamicInsert = false;
				dynamicUpdate = false;
				optimisticLockType = OptimisticLockType.Version;
				polymorphismType = PolymorphismType.Implicit;
				selectBeforeUpdate = false;
			}
		}

		public void SetDiscriminatorValue(String discriminatorValue)
		{
			this.discriminatorValue = discriminatorValue;
		}

		public void BindEntity()
		{
			persistentClass.IsAbstract = annotatedClass.IsAbstract;
			persistentClass.ClassName = annotatedClass.Name;
			persistentClass.NodeName = name;
			//TODO:review this
			//persistentClass.setDynamic(false); //no longer needed with the Entity name refactoring?
			persistentClass.EntityName = (annotatedClass.Name);
			BindDiscriminatorValue();

			persistentClass.IsLazy = lazy;
			if (proxyClass != null)
			{
				persistentClass.ProxyInterfaceName = proxyClass.Name;
			}
			persistentClass.DynamicInsert = dynamicInsert;
			persistentClass.DynamicUpdate = dynamicUpdate;

			if (persistentClass is RootClass)
			{
				RootClass rootClass = (RootClass)persistentClass;
				bool mutable = true;
				//priority on @Immutable, then @Entity.mutable()
				if (annotatedClass.IsAttributePresent<ImmutableAttribute>())
				{
					mutable = false;
				}
				else
				{
					var entityAnn = annotatedClass.GetAttribute<EntityAttribute>();
					if (entityAnn != null)
					{
						mutable = entityAnn.IsMutable;
					}
				}
				rootClass.IsMutable = mutable;
				rootClass.IsExplicitPolymorphism = ExplicitPolymorphismConverter.Convert(polymorphismType);
				if (StringHelper.IsNotEmpty(where)) rootClass.Where = where;

				if (cacheConcurrentStrategy != null)
				{
					rootClass.CacheConcurrencyStrategy = cacheConcurrentStrategy;
					rootClass.CacheRegionName = cacheRegion;
					//TODO: LazyPropertiesCacheable
					//rootClass.LazyPropertiesCacheable =  cacheLazyProperty ;
				}
				rootClass.IsForceDiscriminator = annotatedClass.IsAttributePresent<ForceDiscriminatorAttribute>();
			}
			else
			{
				if (explicitHibernateEntityAnnotation)
				{
					log.WarnFormat("[NHibernate.Annotations.Entity] used on a non root entity: ignored for {0}", annotatedClass.Name);
				}
				if (annotatedClass.IsAttributePresent<ImmutableAttribute>())
				{
					log.WarnFormat("[Immutable] used on a non root entity: ignored for {0}", annotatedClass.Name);
				}
			}
			persistentClass.OptimisticLockMode = OptimisticLockModeConverter.Convert(optimisticLockType);
			persistentClass.SelectBeforeUpdate = selectBeforeUpdate;

			//set persister if needed
			//[Persister] has precedence over [Entity.persister]
			var persisterAnn = annotatedClass.GetAttribute<PersisterAttribute>();
			System.Type persister = null;
			if (persisterAnn != null)
			{
				persister = persisterAnn.Implementation;
			}
			else
			{
				var entityAnn = annotatedClass.GetAttribute<EntityAttribute>();
				if (entityAnn != null && !BinderHelper.IsDefault(entityAnn.Persister))
				{
					try
					{
						persister = ReflectHelper.ClassForName(entityAnn.Persister);
					}
					catch (TypeLoadException tle)
					{
						throw new AnnotationException("Could not find persister class: " + persister, tle);
					}
				}
			}
			if (persister != null) persistentClass.EntityPersisterClass = persister;
			persistentClass.BatchSize = batchSize;

			//SQL overriding
			var sqlInsert = annotatedClass.GetAttribute<SQLInsertAttribute>();
			var sqlUpdate = annotatedClass.GetAttribute<SQLUpdateAttribute>();
			var sqlDelete = annotatedClass.GetAttribute<SQLDeleteAttribute>();
			var sqlDeleteAll = annotatedClass.GetAttribute<SQLDeleteAllAttribute>();
			var loader = annotatedClass.GetAttribute<LoaderAttribute>();

			if (sqlInsert != null)
			{
				persistentClass.SetCustomSQLInsert(sqlInsert.Sql.Trim(), sqlInsert.Callable,
												   ExecuteUpdateResultCheckStyleConverter.Convert(sqlInsert.Check));
			}
			if (sqlUpdate != null)
			{
				persistentClass.SetCustomSQLUpdate(sqlUpdate.Sql.Trim(), sqlUpdate.Callable,
												   ExecuteUpdateResultCheckStyleConverter.Convert(sqlUpdate.Check));
			}
			if (sqlDelete != null)
			{
				persistentClass.SetCustomSQLDelete(sqlDelete.Sql, sqlDelete.Callable,
												   ExecuteUpdateResultCheckStyleConverter.Convert(sqlDelete.Check));
			}
			if (sqlDeleteAll != null)
			{
				persistentClass.SetCustomSQLDelete(sqlDeleteAll.Sql, sqlDeleteAll.Callable,
												   ExecuteUpdateResultCheckStyleConverter.Convert(sqlDeleteAll.Check));
			}
			if (loader != null)
			{
				persistentClass.LoaderName = loader.NamedQuery;
			}

			//tuplizers
			if (annotatedClass.IsAttributePresent<TuplizerAttribute>())
			{
				foreach (TuplizerAttribute tuplizer in annotatedClass.GetAttributes<TuplizerAttribute>())
				{
					var mode = EntityModeConverter.Convert(tuplizer.EntityMode);
					persistentClass.AddTuplizer(mode, tuplizer.Implementation.Name);
				}
			}
			if (annotatedClass.IsAttributePresent<TuplizerAttribute>())
			{
				var tuplizer = annotatedClass.GetAttribute<TuplizerAttribute>();
				var mode = EntityModeConverter.Convert(tuplizer.EntityMode);
				persistentClass.AddTuplizer(mode, tuplizer.Implementation.Name);
			}

			if (!inheritanceState.HasParents)
			{
				var iter = filters.GetEnumerator();
				while (iter.MoveNext())
				{
					var filter = iter.Current;
					String filterName = filter.Key;
					String cond = filter.Value;
					if (BinderHelper.IsDefault(cond))
					{
						FilterDefinition definition = mappings.GetFilterDefinition(filterName);
						cond = definition == null ? null : definition.DefaultFilterCondition;
						if (StringHelper.IsEmpty(cond))
							throw new AnnotationException("no filter condition found for filter " + filterName + " in " + name);
					}
					persistentClass.AddFilter(filterName, cond);
				}
			}
			else
			{
				if (filters.Count > 0)
				{
					log.WarnFormat("@Filter not allowed on subclasses (ignored): {0}", persistentClass.EntityName);
				}
			}
			log.DebugFormat("Import with entity name {0}", name);

			try
			{
				mappings.AddImport(persistentClass.EntityName, name);
				String entityName = persistentClass.EntityName;
				if (!entityName.Equals(name))
				{
					mappings.AddImport(entityName, entityName);
				}
			}
			catch (MappingException me)
			{
				throw new AnnotationException("Use of the same entity name twice: " + name, me);
			}
		}

		private bool IsExplicitPolymorphism(PolymorphismType type)
		{
			throw new NotImplementedException();
		}

		private void BindDiscriminatorValue()
		{
			if (StringHelper.IsEmpty(discriminatorValue))
			{
				IValue discriminator = persistentClass.Discriminator;
				if (discriminator == null)
				{
					persistentClass.DiscriminatorValue = name;
				}
				else if ("character".Equals(discriminator.GetType().Name))
				{
					throw new AnnotationException("Using default @DiscriminatorValue for a discriminator of type CHAR is not safe");
				}
				else if ("integer".Equals(discriminator.GetType().Name))
				{
					//TODO: review if it's correct
					//persistentClass.DiscriminatorValue = String.valueOf(name.GetHashCode());
					persistentClass.DiscriminatorValue = name.GetHashCode().ToString();
				}
				else
				{
					persistentClass.DiscriminatorValue = name; //Spec compliant
				}
			}
			else
			{
				//persistentClass.getDiscriminator()
				persistentClass.DiscriminatorValue = discriminatorValue;
			}
		}

		//public void setBatchSize(BatchSize sizeAnn)
		//{
		//    if (sizeAnn != null)
		//    {
		//        batchSize = sizeAnn.size();
		//    }
		//    else
		//    {
		//        batchSize = -1;
		//    }
		//}

		//public void setProxy(Proxy proxy)
		//{
		//    if (proxy != null)
		//    {
		//        lazy = proxy.lazy();
		//        if (!lazy)
		//        {
		//            proxyClass = null;
		//        }
		//        else
		//        {
		//            if (AnnotationBinder.isDefault(
		//                    mappings.getReflectionManager().toXClass(proxy.proxyClass()), mappings
		//            ))
		//            {
		//                proxyClass = annotatedClass;
		//            }
		//            else
		//            {
		//                proxyClass = mappings.getReflectionManager().toXClass(proxy.proxyClass());
		//            }
		//        }
		//    }
		//    else
		//    {
		//        lazy = true; //needed to allow association lazy loading.
		//        proxyClass = annotatedClass;
		//    }
		//}

		//public void setWhere(Where whereAnn)
		//{
		//    if (whereAnn != null)
		//    {
		//        where = whereAnn.clause();
		//    }
		//}

		private string GetClassTableName(string tableName)
		{
			if (StringHelper.IsEmpty(tableName))
				return mappings.NamingStrategy.ClassToTableName(name);
			else
				return mappings.NamingStrategy.TableName(tableName);
		}

		public void BindTable(string schema, string catalog,
							  string tableName, IList<String[]> uniqueConstraints,
							  string constraints, Table denormalizedSuperclassTable)
		{
			String logicalName = StringHelper.IsNotEmpty(tableName) ? tableName : StringHelper.Unqualify(name);
			Table table = TableBinder.FillTable(schema, catalog, GetClassTableName(tableName), logicalName,
												persistentClass.IsAbstract, uniqueConstraints, constraints,
												denormalizedSuperclassTable, mappings);
			if (persistentClass is ITableOwner)
			{
				log.InfoFormat("Bind entity {0} on table {1}", persistentClass.EntityName, table.Name);
				((ITableOwner)persistentClass).Table = table;
			}
			else
			{
				throw new AssertionFailure("Binding a table for a subclass");
			}
		}

		public void FinalSecondaryTableBinding(IPropertyHolder propertyHolder)
		{
			/*
			 * Those operations has to be done after the id definition of the persistence class.
			 * ie after the properties parsing
			 */
			var joins = secondaryTables.Values.GetEnumerator();
			var joinColumns = secondaryTableJoins.Values.GetEnumerator();

			while (joins.MoveNext())
			{
				joinColumns.MoveNext();
				var uncastedColumn = joinColumns.Current;
				var join = joins.Current;
				CreatePrimaryColumnsToSecondaryTable(uncastedColumn, propertyHolder, join);
			}
			mappings.AddJoins(persistentClass, secondaryTables);
		}

		private void CreatePrimaryColumnsToSecondaryTable(object uncastedColumn, IPropertyHolder propertyHolder, Join join)
		{
			Ejb3JoinColumn[] ejb3JoinColumns;
			PrimaryKeyJoinColumnAttribute[] pkColumnsAnn = null;
			JoinColumnAttribute[] joinColumnsAnn = null;
			if (uncastedColumn is PrimaryKeyJoinColumnAttribute[])
			{
				pkColumnsAnn = (PrimaryKeyJoinColumnAttribute[])uncastedColumn;
			}
			if (uncastedColumn is JoinColumnAttribute[])
			{
				joinColumnsAnn = (JoinColumnAttribute[])uncastedColumn;
			}
			if (pkColumnsAnn == null && joinColumnsAnn == null)
			{
				ejb3JoinColumns = new Ejb3JoinColumn[1];
				ejb3JoinColumns[0] = Ejb3JoinColumn.BuildJoinColumn(
						null,
						null,
						persistentClass.Identifier,
						secondaryTables,
						propertyHolder, mappings
				);
			}
			else
			{
				int nbrOfJoinColumns = pkColumnsAnn != null ?
						pkColumnsAnn.Length :
						joinColumnsAnn.Length;
				if (nbrOfJoinColumns == 0)
				{
					ejb3JoinColumns = new Ejb3JoinColumn[1];
					ejb3JoinColumns[0] = Ejb3JoinColumn.BuildJoinColumn(
							null,
							null,
							persistentClass.Identifier,
							secondaryTables,
							propertyHolder, mappings
					);
				}
				else
				{
					ejb3JoinColumns = new Ejb3JoinColumn[nbrOfJoinColumns];
					if (pkColumnsAnn != null)
					{
						for (int colIndex = 0; colIndex < nbrOfJoinColumns; colIndex++)
						{
							ejb3JoinColumns[colIndex] = Ejb3JoinColumn.BuildJoinColumn(
									pkColumnsAnn[colIndex],
									null,
									persistentClass.Identifier,
									secondaryTables,
									propertyHolder, mappings
							);
						}
					}
					else
					{
						for (int colIndex = 0; colIndex < nbrOfJoinColumns; colIndex++)
						{
							ejb3JoinColumns[colIndex] = Ejb3JoinColumn.BuildJoinColumn(
									null,
									joinColumnsAnn[colIndex],
									persistentClass.Identifier,
									secondaryTables,
									propertyHolder, mappings
							);
						}
					}
				}
			}

			foreach (Ejb3JoinColumn joinColumn in ejb3JoinColumns)
			{
				joinColumn.ForceNotNull();
			}
			BindJoinToPersistentClass(join, ejb3JoinColumns);
		}

		private void BindJoinToPersistentClass(Join join, Ejb3JoinColumn[] ejb3JoinColumns)
		{
			SimpleValue key = new DependantValue(join.Table, persistentClass.Identifier);
			join.Key = key;
			SetFKNameIfDefined(join);
			key.IsCascadeDeleteEnabled = false;
			TableBinder.BindFk(persistentClass, null, ejb3JoinColumns, key, false, mappings);
			join.CreatePrimaryKey(dialect);
			join.CreateForeignKey();
			persistentClass.AddJoin(join);
		}

		private void SetFKNameIfDefined(Join join)
		{
			TableAttribute matchingTable = FindMatchingComplimentTableAnnotation(join);
			if (matchingTable != null && !BinderHelper.IsDefault(matchingTable.ForeignKey.Name))
			{
				((SimpleValue)join.Key).ForeignKeyName = matchingTable.ForeignKey.Name;
			}
		}

		private TableAttribute FindMatchingComplimentTableAnnotation(Join join)
		{
			String tableName = join.Table.GetQuotedName();
			var table = annotatedClass.GetAttribute<TableAttribute>();
			TableAttribute matchingTable = null;
			if (table != null && tableName.Equals(table.AppliesTo))
			{
				matchingTable = table;
			}
			else
			{
				//TODO: Review the way to treat Multiples attributes
				//Tables tables = annotatedClass.getAnnotation( Tables.class );
				//if ( tables != null ) {
				//    for (org.hibernate.annotations.Table current : tables.value()) {
				//        if ( tableName.equals( current.appliesTo() ) ) {
				//            matchingTable = current;
				//            break;
				//        }
				//    }
				//}
			}
			return matchingTable;
		}

		//public void FirstLevelSecondaryTablesBinding(SecondaryTable secTable, SecondaryTables secTables) 
		//{
		//    if (secTables != null) 
		//    {
		//        //loop through it
		//        for (SecondaryTable tab : secTables.value()) 
		//        {
		//            addJoin( tab, null, null, false );
		//        }
		//    }
		//    else 
		//    {
		//        if ( secTable != null ) addJoin( secTable, null, null, false );
		//    }
		//}

		/// <summary>
		/// Used for @*ToMany @JoinTable
		/// </summary>
		public Join AddJoin(JoinTableAttribute joinTable, IPropertyHolder holder, bool noDelayInPkColumnCreation)
		{
			return AddJoin(null, joinTable, holder, noDelayInPkColumnCreation);
		}

		/// <summary>
		/// A non null propertyHolder means than we process the Pk creation without delay
		/// </summary>
		/// <param name="secondaryTable"></param>
		/// <param name="joinTable"></param>
		/// <param name="propertyHolder"></param>
		/// <param name="noDelayInPkColumnCreation"></param>
		/// <returns></returns>
		private Join AddJoin(SecondaryTableAttribute secondaryTable,
			JoinTableAttribute joinTable,
			IPropertyHolder propertyHolder,
			bool noDelayInPkColumnCreation)
		{
			Join join = new Join();
			join.PersistentClass = persistentClass;
			string schema;
			string catalog;
			string table;
			string realTable;

			System.Persistence.UniqueConstraintAttribute[] uniqueConstraintsAnn;
			if (secondaryTable != null)
			{
				schema = secondaryTable.Schema;
				catalog = secondaryTable.Catalog;
				table = secondaryTable.Name;
				realTable = mappings.NamingStrategy.TableName(table); //always an explicit table name
				uniqueConstraintsAnn = secondaryTable.UniqueConstraints;
			}
			else if (joinTable != null)
			{
				schema = joinTable.Schema;
				catalog = joinTable.Catalog;
				table = joinTable.Name;
				realTable = mappings.NamingStrategy.TableName(table); //always an explicit table name
				uniqueConstraintsAnn = joinTable.UniqueConstraints;
			}
			else
			{
				throw new AssertionFailure("Both JoinTable and SecondaryTable are null");
			}

			var uniqueConstraints = new List<string[]>(uniqueConstraintsAnn == null ? 0 : uniqueConstraintsAnn.Length);
			if (uniqueConstraintsAnn != null && uniqueConstraintsAnn.Length != 0)
			{
				foreach (UniqueConstraintAttribute uc in uniqueConstraintsAnn)
				{
					uniqueConstraints.Add(uc.ColumnNames);
				}
			}
			Table tableMapping = TableBinder.FillTable(
					schema,
					catalog,
					realTable,
					table, false, uniqueConstraints, null, null, mappings);
			//no check constraints available on joins
			join.Table = tableMapping;

			//somehow keep joins() for later.
			//Has to do the work later because it needs persistentClass id!
			object joinColumns = null;
			//get the appropriate pk columns
			if (secondaryTable != null)
			{
				joinColumns = secondaryTable.PkJoinColumns;
			}
			else if (joinTable != null)
			{
				joinColumns = joinTable.JoinColumns;
			}
			log.InfoFormat("Adding secondary table to entity {0} -> {1}", persistentClass.EntityName, join.Table.Name);

			TableAttribute matchingTable = FindMatchingComplimentTableAnnotation(join);

			if (matchingTable != null)
			{
				join.IsSequentialSelect = FetchMode.Join != matchingTable.Fetch;
				join.IsInverse = matchingTable.IsInverse;
				join.IsOptional = matchingTable.IsOptional;
				if (!BinderHelper.IsDefault(matchingTable.SqlInsert.Sql))
				{
					join.SetCustomSQLInsert(matchingTable.SqlInsert.Sql.Trim(),
							matchingTable.SqlInsert.Callable,
							ExecuteUpdateResultCheckStyle.Parse(matchingTable.SqlInsert.Check.ToString().ToLower()));
				}
				if (!BinderHelper.IsDefault(matchingTable.SqlUpdate.Sql))
				{
					join.SetCustomSQLUpdate(matchingTable.SqlUpdate.Sql.Trim(),
							matchingTable.SqlUpdate.Callable,
							ExecuteUpdateResultCheckStyle.Parse(matchingTable.SqlUpdate.Check.ToString().ToLower())
					);
				}
				if (!BinderHelper.IsDefault(matchingTable.SqlDelete.Sql))
				{
					join.SetCustomSQLDelete(matchingTable.SqlDelete.Sql.Trim(),
							matchingTable.SqlDelete.Callable,
							ExecuteUpdateResultCheckStyle.Parse(matchingTable.SqlDelete.Check.ToString().ToLower())
					);
				}
			}
			else
			{
				//default
				join.IsSequentialSelect = false;
				join.IsInverse = false;
				join.IsOptional = false; //perhaps not quite per-spec, but a Good Thing anyway
			}

			if (noDelayInPkColumnCreation)
			{
				CreatePrimaryColumnsToSecondaryTable(joinColumns, propertyHolder, join);
			}
			else
			{
				secondaryTables.Add(table, join);
				secondaryTableJoins.Add(table, joinColumns);
			}
			return join;
		}

		public IDictionary<string, Join> SecondaryTables
		{
			get { return secondaryTables; }
		}


	}
}