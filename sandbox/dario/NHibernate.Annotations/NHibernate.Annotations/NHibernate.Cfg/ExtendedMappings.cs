using System.Collections.Generic;
using log4net;
using NHibernate.Annotations;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Annotations.NHibernate.Mapping;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Mapping;
using Iesi.Collections.Generic;

namespace NHibernate.Cfg
{
	public class ExtendedMappings : Mappings
	{
		private readonly ILog log = LogManager.GetLogger(typeof(ExtendedMappings));
		private readonly Dictionary<string, IdGenerator> namedGenerators;
		private readonly Dictionary<string, Dictionary<string, Join>> joins;
		private readonly Dictionary<string, AnnotatedClassType> classTypes;
		//private readonly Dictionary<string, Properties> generatorTables;
		private readonly Dictionary<Table, List<string[]>> tableUniqueConstraints;
		private readonly Dictionary<string, string> MappedByResolver;
		private readonly Dictionary<string, string> propertyRefResolver;
		//private readonly ReflectionManager reflectionManager;
		private readonly ISet<string> defaultNamedQueryNames;
		private readonly ISet<string> defaultNamedNativeQueryNames;
		private readonly ISet<string> defaultSqlResulSetMappingNames;
		private readonly ISet<string> defaultNamedGenerators;
		private readonly Dictionary<string, AnyMetaDefAttribute> anyMetaDefs;

		public ExtendedMappings(IDictionary<string, 
			PersistentClass> classes, 
			IDictionary<string, Mapping.Collection> collections, 
			IDictionary<string, Table> tables, 
			IDictionary<string, NamedQueryDefinition> queries, 
			IDictionary<string, NamedSQLQueryDefinition> sqlqueries, 
			IDictionary<string, ResultSetMappingDefinition> resultSetMappings, 
			IDictionary<string, string> imports, IList<SecondPassCommand> secondPasses, 
			IList<PropertyReference> propertyReferences, 
			INamingStrategy namingStrategy, 
			IDictionary<string, TypeDef> typeDefs, 
			IDictionary<string, FilterDefinition> filterDefinitions, 
			ISet<ExtendsQueueEntry> extendsQueue, 
			IList<IAuxiliaryDatabaseObject> auxiliaryDatabaseObjects, 
			IDictionary<string, TableDescription> tableNameBinding, 
			IDictionary<Table, ColumnNames> columnNameBindingPerTable, 
			string defaultAssembly, string defaultNamespace, 
			Dialect.Dialect dialect, Dictionary<string, IdGenerator> namedGenerators, 
			Dictionary<string, Dictionary<string, Join>> joins, 
			Dictionary<string, AnnotatedClassType> classTypes, 
			Dictionary<Table, List<string[]>> tableUniqueConstraints, 
			Dictionary<string, string> mappedByResolver, 
			Dictionary<string, string> propertyRefResolver, 
			ISet<string> defaultNamedQueryNames, ISet<string> defaultNamedNativeQueryNames, 
			ISet<string> defaultSqlResulSetMappingNames, 
			ISet<string> defaultNamedGenerators, 
			Dictionary<string, AnyMetaDefAttribute> anyMetaDefs) : 
			base(classes, collections, tables, queries, sqlqueries, resultSetMappings, imports, secondPasses, propertyReferences, namingStrategy, typeDefs, filterDefinitions, extendsQueue, auxiliaryDatabaseObjects, tableNameBinding, columnNameBindingPerTable, defaultAssembly, defaultNamespace, dialect)
		{
			this.namedGenerators = namedGenerators;
			this.joins = joins;
			this.classTypes = classTypes;
			this.tableUniqueConstraints = tableUniqueConstraints;
			MappedByResolver = mappedByResolver;
			this.propertyRefResolver = propertyRefResolver;
			this.defaultNamedQueryNames = defaultNamedQueryNames;
			this.defaultNamedNativeQueryNames = defaultNamedNativeQueryNames;
			this.defaultSqlResulSetMappingNames = defaultSqlResulSetMappingNames;
			this.defaultNamedGenerators = defaultNamedGenerators;
			this.anyMetaDefs = anyMetaDefs;
		}
	}
}
