using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Properties;
using NHibernate.Engine;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Reader;
using NHibernate.Collection;
using System.Runtime.Serialization;

namespace NHibernate.Envers.Entities.Mapper.Relation
{
    /**
     * @author Catalina Panait, port of Envers omonyme class by Adam Warski (adam at warski dot org)
     */

public class OneToOneNotOwningMapper : IPropertyMapper {
    private String owningReferencePropertyName;
    private String owningEntityName;
    private PropertyData propertyData;

    public OneToOneNotOwningMapper(String owningReferencePropertyName, String owningEntityName,
                                   PropertyData propertyData) {
        this.owningReferencePropertyName = owningReferencePropertyName;
        this.owningEntityName = owningEntityName;
        this.propertyData = propertyData;
    }

    public bool MapToMapFromEntity(ISessionImplementor session, IDictionary<String, Object> data, Object newObj, Object oldObj) {
        return false;
    }

    public void MapToEntityFromMap(AuditConfiguration verCfg, Object obj, IDictionary<String, Object> data, Object primaryKey, IAuditReaderImplementor versionsReader, long revision) 
    {
        //TODO in implementing second phase/////////////
        throw new NotImplementedException("Not Implemented!");

        //if (obj == null) {
        //    return;
        //}Not implememented

        //System.Type entityClass = ReflectionTools.loadClass(owningEntityName); 

        //Object value;

        //try {
        //    value = versionsReader.CreateQuery().ForEntitiesAtRevision(entityClass, revision)
        //            //.Add( (AuditEntity.relatedId(owningReferencePropertyName).eq(primaryKey)).getSingleResult();
        //} catch (NoResultException e) {
        //    value = null;
        //} catch (NonUniqueResultException e) {
        //    throw new AuditException("Many versions results for one-to-one relationship: (" + owningEntityName +
        //            ", " + owningReferencePropertyName + ")");
        //}

        //ISetter setter = ReflectionTools.getSetter(obj.getClass(), propertyData); 
        //Catalina: la portare se foloseste PropertyInfo. Exemple in MultiPropertyMapper si SinglePropertyMapper
        //setter.set(obj, value, null);

    }

    public IList<PersistentCollectionChangeData> MapCollectionChanges(String referencingPropertyName,
                                                                                    IPersistentCollection newColl,
                                                                                    object oldColl,
                                                                                    object id)
    {
        return null;
    }

    #region IPropertyMapper Members

    public void MapToEntityFromMap(NHibernate.Envers.Configuration.AuditConfiguration verCfg, object obj, IDictionary<object, object> data, object primaryKey, NHibernate.Envers.Reader.IAuditReaderImplementor versionsReader, long revision)
    {
       // //TODO in implementing second phase/////////////
       //if (obj == null) {
       //     return;
       // }

       ////System.Type entityClass = ReflectionTools.loadClass(owningEntityName);
       // System.Runtime.Remoting.ObjectHandle hdl = Activator.CreateInstance(owningEntityName);
       // System.Type entityClass = hdl.GetType();
       // Object value;

       // try {
       //     value = versionsReader.CreateQuery().ForEntitiesAtRevision(entityClass, revision)
       //             .Add(AuditEntity.relatedId(owningReferencePropertyName).eq(primaryKey)).getSingleResult();
       // } catch (NoResultException e) {
       //     value = null;
       // } catch (NonUniqueResultException e) {
       //     throw new AuditException("Many versions results for one-to-one relationship: (" + owningEntityName +
       //             ", " + owningReferencePropertyName + ")");
       // }

       // Setter setter = ReflectionTools.getSetter(obj.getClass(), propertyData);
       // setter.set(obj, value, null);
    }


    #endregion
}

}
