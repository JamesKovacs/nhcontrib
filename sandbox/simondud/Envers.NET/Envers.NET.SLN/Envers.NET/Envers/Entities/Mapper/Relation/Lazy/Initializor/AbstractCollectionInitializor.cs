using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Query;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor
{
    /**
     * Initializes a persistent collection.
     * @author Adam Warski (adam at warski dot org)
     */
    public abstract class AbstractCollectionInitializor<T> : IInitializor<T> {
        private readonly IAuditReaderImplementor versionsReader;
        private readonly IRelationQueryGenerator queryGenerator;
        private readonly Object primaryKey;
        
        protected readonly long revision;
        protected readonly EntityInstantiator entityInstantiator;

        public AbstractCollectionInitializor(AuditConfiguration verCfg,
                                             IAuditReaderImplementor versionsReader,
                                             IRelationQueryGenerator queryGenerator,
                                             Object primaryKey, long revision) {
            this.versionsReader = versionsReader;
            this.queryGenerator = queryGenerator;
            this.primaryKey = primaryKey;
            this.revision = revision;

            entityInstantiator = new EntityInstantiator(verCfg, versionsReader);
        }

        protected abstract T InitializeCollection(int size);

        protected abstract void AddToCollection(T collection, Object collectionRow);

        public T Initialize() {
            IList<object> collectionContent = queryGenerator.GetQuery(versionsReader, primaryKey, revision).List<object>();

            T collection = InitializeCollection(collectionContent.Count);

            foreach (Object collectionRow in collectionContent) {
                AddToCollection(collection, collectionRow);
            }

            return collection;
        }
    }
}
