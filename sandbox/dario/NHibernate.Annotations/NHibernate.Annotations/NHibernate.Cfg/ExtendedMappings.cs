using System.Collections.Generic;
using Iesi.Collections.Generic;
using log4net;
using NHibernate.Annotations;
using NHibernate.Annotations.NHibernate.Mapping;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace NHibernate.Cfg
{
    public class ExtendedMappings : Mappings
    {
        private readonly ILog log = LogManager.GetLogger(typeof (ExtendedMappings));
        private readonly IDictionary<string, IdGenerator> namedGenerators;
        private readonly IDictionary<string, IDictionary<string, Join>> joins;
        private readonly IDictionary<string, AnnotatedClassType> classTypes;
        private readonly Dictionary<string, Annotations.Properties> generatorTables;
        private readonly IDictionary<Table, List<string[]>> tableUniqueConstraints;
        private readonly IDictionary<string, string> MappedByResolver;
        private readonly IDictionary<string, string> propertyRefResolver;
        private readonly ISet<string> defaultNamedQueryNames;
        private readonly ISet<string> defaultNamedNativeQueryNames;
        private readonly ISet<string> defaultSqlResulSetMappingNames;
        private readonly ISet<string> defaultNamedGenerators;
        private readonly IDictionary<string, AnyMetaDefAttribute> anyMetaDefs;
        //private readonly ReflectionManager reflectionManager;

           public ExtendedMappings(IDictionary<string, PersistentClass> classes,
                                IDictionary<string, Mapping.Collection> collections,
                                IDictionary<string, Table> tables,
                                IDictionary<string, NamedQueryDefinition> queries,
                                IDictionary<string, NamedSQLQueryDefinition> sqlqueries,
                                IDictionary<string, ResultSetMappingDefinition> resultSetMappings,
                                ISet<string> defaultNamedQueryNames,
                                ISet<string> defaultNamedNativeQueryNames,
                                ISet<string> defaultSqlResulSetMappingNames,
                                ISet<string> defaultNamedGenerators,
                                IDictionary<string, string> imports, 
                                IList<SecondPassCommand> secondPasses,
                                IList<PropertyReference> propertyReferences,
                                INamingStrategy namingStrategy,
                                IDictionary<string, TypeDef> typeDefs,
                                IDictionary<string, FilterDefinition> filterDefinitions,
                                IDictionary<string, IdGenerator> namedGenerators,
                                IDictionary<string, IDictionary<string, Join>> joins,
                                IDictionary<string, AnnotatedClassType> classTypes,
                                ISet<ExtendsQueueEntry> extendsQueue,
                                IDictionary<string, TableDescription> tableNameBinding,
                                IDictionary<Table, ColumnNames> columnNameBindingPerTable,
                                IList<IAuxiliaryDatabaseObject> auxiliaryDatabaseObjects,
                                IDictionary<string,Annotations.Properties> generatorTables,
                                IDictionary<Table, List<string[]>> tableUniqueConstraints,
                                IDictionary<string, string> mappedByResolver,
                                IDictionary<string, string> propertyRefResolver,
                                IDictionary<string, AnyMetaDefAttribute> anyMetaDefs,
                                string defaultAssembly, 
                                string defaultNamespace,
                                Dialect.Dialect dialect) :
                                    base(classes,
                                         collections,
                                         tables,
                                         queries,
                                         sqlqueries,
                                         resultSetMappings,
                                         imports,
                                         secondPasses,
                                         propertyReferences,
                                         namingStrategy,
                                         typeDefs,
                                         filterDefinitions,
                                         extendsQueue,
                                         auxiliaryDatabaseObjects,
                                         tableNameBinding,
                                         columnNameBindingPerTable,
                                         defaultAssembly,
                                         defaultNamespace,
                                         dialect)
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


        public IDictionary<string, PersistentClass> Classes
        {
            get { return null; }
        }

        public void AddSecondPass(IndexOrUniqueKeySecondPass secondPass)
        {
        }

		public void AddJoins(PersistentClass persistentClass, IDictionary<string, Join> joins) 
		{
			if(this.joins.ContainsKey(persistentClass.EntityName))
				log.WarnFormat("duplicate joins for class: {0}", persistentClass.EntityName);

			this.joins.Add(persistentClass.EntityName, joins);
		}
    }
}