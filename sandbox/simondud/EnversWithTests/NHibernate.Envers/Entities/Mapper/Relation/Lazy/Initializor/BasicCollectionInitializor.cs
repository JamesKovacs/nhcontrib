using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Iesi.Collections;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Query;
using NHibernate.Envers.Exceptions;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy.Initializor
{
    public class BasicCollectionInitializor<T> : AbstractCollectionInitializor<T>
	{
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
            this.collectionType = collectionType.MakeGenericType(typeof(T));
            this.elementComponentData = elementComponentData;
        }

        protected override ICollection<T> InitializeCollection(int size) 
		{
            try
            {
                return (ICollection<T>) Activator.CreateInstance(collectionType);
            } 
			catch (InstantiationException e) 
			{
                throw new AuditException(e);
            } 
			catch (SecurityException e) 
			{
                throw new AuditException(e);
            }
        }

        protected override void AddToCollection(ICollection<T> collection, Object collectionRow) 
		{
            var elementData = ((IList) collectionRow)[elementComponentData.ComponentIndex];

            // If the target entity is not audited, the elements may be the entities already, so we have to check
            // if they are maps or not.
            T element;
            if (elementData is IDictionary) 
			{
                element = (T)elementComponentData.ComponentMapper.MapToObjectFromFullMap(entityInstantiator,
                        (IDictionary<String, Object>) elementData, null, revision);
            } 
			else 
			{
                element = (T)elementData;
            }
			collection.Add(element);
        }
    }
}
