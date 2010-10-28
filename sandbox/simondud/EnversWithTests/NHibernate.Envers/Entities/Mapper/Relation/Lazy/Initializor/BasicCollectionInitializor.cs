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

    /**
     * Initializes a non-indexed java collection (set or list, eventually sorted).
     * T has to implement ICollection.
     * @author Adam Warski (adam at warski dot org)
     */
    public class BasicCollectionInitializor<T> : AbstractCollectionInitializor<T>
	{
        private readonly System.Type collectionType;
        private readonly MiddleComponentData elementComponentData;
    	private readonly System.Type[] _genericArguments;

    	public BasicCollectionInitializor(AuditConfiguration verCfg,
											IAuditReaderImplementor versionsReader,
											IRelationQueryGenerator queryGenerator,
											Object primaryKey, long revision,
											System.Type collectionType,
											MiddleComponentData elementComponentData,
											System.Type[] genericArguments) 
								:base(verCfg, versionsReader, queryGenerator, primaryKey, revision)
        {
            this.collectionType = collectionType;
            this.elementComponentData = elementComponentData;
        	_genericArguments = genericArguments;
        }

        protected override ICollection<T> InitializeCollection(int size) 
		{
            try
            {
				var collType = collectionType.MakeGenericType(_genericArguments);
                return (ICollection<T>) Activator.CreateInstance(collType);
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

        protected override void AddToCollection(object collection, Object collectionRow) 
		{
            object elementData = ((IList) collectionRow)[elementComponentData.ComponentIndex];

            // If the target entity is not audited, the elements may be the entities already, so we have to check
            // if they are maps or not.
            object element;
            if (elementData is IDictionary) 
			{
                element = elementComponentData.ComponentMapper.MapToObjectFromFullMap(entityInstantiator,
                        (IDictionary<String, Object>) elementData, null, revision);
            } 
			else 
			{
                element = elementData;
            }

			//rk fix this, should of course not only support sets!
			((ISet) collection).Add(element);
        }
    }
}
