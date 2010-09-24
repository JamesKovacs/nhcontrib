using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Query;
using NHibernate.Envers.Exceptions;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor
{

    /**
     * Initializes a non-indexed java collection (set or list, eventually sorted).
     * T has to implement ICollection.
     * @author Adam Warski (adam at warski dot org)
     */
    public class BasicCollectionInitializor<T>: AbstractCollectionInitializor<T> {
        private readonly System.Type collectionType;
        private readonly MiddleComponentData elementComponentData;

        public BasicCollectionInitializor(AuditConfiguration verCfg,
                                           IAuditReaderImplementor versionsReader,
                                           IRelationQueryGenerator queryGenerator,
                                           Object primaryKey, long revision,
                                           System.Type collectionType,
                                           MiddleComponentData elementComponentData) 
            :base(verCfg, versionsReader, queryGenerator, primaryKey, revision)
        {
            if (! (typeof(T).IsSubclassOf(typeof(ICollection)))) throw new NotSupportedException("Type supplied has to be derived from ICollection!");
            this.collectionType = collectionType;
            this.elementComponentData = elementComponentData;
        }

        protected override T InitializeCollection(int size) {
            try {
                return (T)Activator.CreateInstance(collectionType);
            } catch (InstantiationException e) {
                throw new AuditException(e);
            } catch (SecurityException e) {
                throw new AuditException(e);
            }
        }

        protected override void AddToCollection(T collection, Object collectionRow) {
            Object elementData = ((IList) collectionRow)[elementComponentData.ComponentIndex];

            // If the target entity is not audited, the elements may be the entities already, so we have to check
            // if they are maps or not.
            Object element;
            if (elementData is IDictionary) {
                element = elementComponentData.ComponentMapper.MapToObjectFromFullMap(entityInstantiator,
                        (IDictionary<String, Object>) elementData, null, revision);
            } else {
                element = elementData;
            }
            throw new NotImplementedException("work in progress, figuring out next line");
            //((ICollection)collection).Add(element);
        }
    }
}
