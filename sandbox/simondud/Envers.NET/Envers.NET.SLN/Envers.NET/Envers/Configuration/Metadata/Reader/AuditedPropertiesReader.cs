using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using NHibernate.Envers.Tools;
using NHibernate.Mapping;
using System.Reflection;
using NHibernate.Envers.Compatibility.Attributes;

namespace NHibernate.Envers.Configuration.Metadata.Reader
{
    /**
     * Reads persistent properties form a
     * {@link org.hibernate.envers.configuration.metadata.reader.PersistentPropertiesSource}
     * and adds the ones that are audited to a
     * {@link org.hibernate.envers.configuration.metadata.reader.AuditedPropertiesHolder},
     * filling all the auditing data.
     * @author Simon Duduica, port of Envers Tools class by Adam Warski (adam at warski dot org)
     * @author Erik-Berndt Scheper
     */
    public class AuditedPropertiesReader {
	    private ModificationStore defaultStore;
	    private IPersistentPropertiesSource persistentPropertiesSource;
	    private IAuditedPropertiesHolder auditedPropertiesHolder;
	    private GlobalConfiguration globalCfg;
	    private String propertyNamePrefix;

	    private ISet<String> propertyAccessedPersistentProperties;
	    private ISet<String> fieldAccessedPersistentProperties;

	    public AuditedPropertiesReader(ModificationStore defaultStore,
								       IPersistentPropertiesSource persistentPropertiesSource,
								       IAuditedPropertiesHolder auditedPropertiesHolder,
								       GlobalConfiguration globalCfg,
								       String propertyNamePrefix) {
		    this.defaultStore = defaultStore;
		    this.persistentPropertiesSource = persistentPropertiesSource;
		    this.auditedPropertiesHolder = auditedPropertiesHolder;
		    this.globalCfg = globalCfg;
		    this.propertyNamePrefix = propertyNamePrefix;

		    propertyAccessedPersistentProperties = Toolz.newHashSet<String>();
		    fieldAccessedPersistentProperties = Toolz.newHashSet<String>();
	    }

	    public void read() {
		    // First reading the access types for the persistent properties.
		    readPersistentPropertiesAccess();

		    // Adding all properties from the given class.
		    addPropertiesFromClass(persistentPropertiesSource.GetClass());
	    }

	    private void readPersistentPropertiesAccess() {
		    IEnumerator<Property> propertyIter = persistentPropertiesSource.PropertyEnumerator;
		    while (propertyIter.MoveNext()) {
			    Property property = (Property) propertyIter.Current;
			    if ("field".Equals(property.PropertyAccessorName)) {
				    fieldAccessedPersistentProperties.Add(property.Name);
			    } else {
				    propertyAccessedPersistentProperties.Add(property.Name);
			    }
		    }
	    }

	    private void addPropertiesFromClass(System.Type clazz)  {
		    System.Type superclazz = clazz.BaseType;
            if (!"System.Object".Equals(superclazz.FullName))
            {
			    addPropertiesFromClass(superclazz);
		    }

            //ORIG: addFromProperties(clazz.getDeclaredProperties("field"), "field", fieldAccessedPersistentProperties);
            //addFromProperties(clazz.getDeclaredProperties("property"), "property", propertyAccessedPersistentProperties);
            addFromProperties(clazz.GetProperties(BindingFlags.GetField), "field", fieldAccessedPersistentProperties);
            addFromProperties(clazz.GetProperties(BindingFlags.GetProperty), "property", propertyAccessedPersistentProperties);
	    }

	    private void addFromProperties(IEnumerable<PropertyInfo> properties, String accessType, ISet<String> persistentProperties) {
		    //ORIG: foreach (XProperty property in properties) {
			foreach (PropertyInfo property in properties) {
			    // If this is not a persistent property, with the same access type as currently checked,
			    // it's not audited as well.
			    if (persistentProperties.Contains(property.Name)) {
				    IValue propertyValue = persistentPropertiesSource.GetProperty(property.Name).Value;

				    PropertyAuditingData propertyData;
				    bool isAudited;
				    if (propertyValue is Component) {
					    ComponentAuditingData componentData = new ComponentAuditingData();
					    isAudited = fillPropertyData(property, componentData, accessType);

					    IPersistentPropertiesSource componentPropertiesSource = new ComponentPropertiesSource(
							    (Component) propertyValue);
					    new AuditedPropertiesReader(ModificationStore.FULL, componentPropertiesSource, componentData,
							    globalCfg,
							    propertyNamePrefix + MappingTools.createComponentPrefix(property.Name))
							    .read();

					    propertyData = componentData;
				    } else {
					    propertyData = new PropertyAuditingData();
					    isAudited = fillPropertyData(property, propertyData, accessType);
				    }

				    if (isAudited) {
					    // Now we know that the property is audited
					    auditedPropertiesHolder.addPropertyAuditingData(property.Name, propertyData);
				    }
			    }
		    }
	    }

	    /**
	     * Checks if a property is audited and if yes, fills all of its data.
	     * @param property Property to check.
	     * @param propertyData Property data, on which to set this property's modification store.
	     * @param accessType Access type for the property.
	     * @return False if this property is not audited.
	     */
	    private bool fillPropertyData(PropertyInfo property, PropertyAuditingData propertyData,
									     String accessType) {

		    // check if a property is declared as not audited to exclude it
		    // useful if a class is audited but some properties should be excluded
            NotAuditedAttribute unVer = (NotAuditedAttribute)Attribute.GetCustomAttribute(property, typeof(NotAuditedAttribute));
		    if (unVer != null) {
			    return false;
		    } else {
			    // if the optimistic locking field has to be unversioned and the current property
			    // is the optimistic locking field, don't audit it
			    if (globalCfg.isDoNotAuditOptimisticLockingField()) {
				    //Version jpaVer = property.getAnnotation(typeof(Version));
                    VersionAttribute jpaVer = (VersionAttribute)Attribute.GetCustomAttribute(property, typeof(VersionAttribute));
				    if (jpaVer != null) {
					    return false;
				    }
			    }
		    }

		    // Checking if this property is explicitly audited or if all properties are.
		    //AuditedAttribute aud = property.getAnnotation(typeof(AuditedAttribute));
            AuditedAttribute aud = (AuditedAttribute)Attribute.GetCustomAttribute(property, typeof(AuditedAttribute));
		    if (aud != null) {
			    propertyData.Store = aud.ModStore;
			    propertyData.setRelationTargetAuditMode(aud.TargetAuditMode);
		    } else {
			    if (defaultStore != null) {
				    propertyData.Store = defaultStore;
			    } else {
				    return false;
			    }
		    }

		    propertyData.Name = propertyNamePrefix + property.Name;
		    propertyData.BeanName = property.Name;
		    propertyData.AccessType = accessType;

		    AddPropertyJoinTables(property, propertyData);
		    AddPropertyAuditingOverrides(property, propertyData);
		    if (!ProcessPropertyAuditingOverrides(property, propertyData)) {
			    return false; // not audited due to AuditOverride annotation
		    }
		    AddPropertyMapKey(property, propertyData);
            SetPropertyAuditMappedBy(property, propertyData);

		    return true;
	    }

        private void SetPropertyAuditMappedBy(PropertyInfo property, PropertyAuditingData propertyData) {
            AuditMappedByAttribute auditMappedBy = (AuditMappedByAttribute)Attribute.GetCustomAttribute(property, typeof(AuditMappedByAttribute));
            if (auditMappedBy != null) {
		        propertyData.AuditMappedBy = auditMappedBy.MappedBy;
                if (!"".Equals(auditMappedBy.PositionMappedBy)) {
                    propertyData.PositionMappedBy = auditMappedBy.PositionMappedBy;
                }
            }
        }

	    private void AddPropertyMapKey(PropertyInfo property, PropertyAuditingData propertyData) {
		    MapKeyAttribute mapKey = (MapKeyAttribute)Attribute.GetCustomAttribute(property, typeof(MapKeyAttribute));
		    if (mapKey != null) {
			    propertyData.MapKey = mapKey.Name;
		    }
	    }

	    private void AddPropertyJoinTables(PropertyInfo property, PropertyAuditingData propertyData) {
		    // first set the join table based on the AuditJoinTable annotation
		    AuditJoinTableAttribute joinTable = (AuditJoinTableAttribute)Attribute.GetCustomAttribute(property, typeof(AuditJoinTableAttribute));;
		    if (joinTable != null) {
			    propertyData.JoinTable = joinTable;
		    } else {
			    propertyData.JoinTable = DEFAULT_AUDIT_JOIN_TABLE;
		    }
	    }

	    /***
	     * Add the {@link org.hibernate.envers.AuditOverride} annotations.
	     *
	     * @param property the property being processed
	     * @param propertyData the Envers auditing data for this property
	     */
	    private void AddPropertyAuditingOverrides(PropertyInfo property, PropertyAuditingData propertyData) {
		    AuditOverrideAttribute annotationOverride = (AuditOverrideAttribute)Attribute.GetCustomAttribute(property, typeof(AuditOverrideAttribute));;
		    if (annotationOverride != null) {
			    propertyData.addAuditingOverride(annotationOverride);
		    }
		    AuditOverridesAttribute annotationOverrides = (AuditOverridesAttribute)Attribute.GetCustomAttribute(property, typeof(AuditOverridesAttribute));;
		    if (annotationOverrides != null) {
			    propertyData.addAuditingOverrides(annotationOverrides);
		    }
	    }

	    /**
	     * Process the {@link org.hibernate.envers.AuditOverride} annotations for this property.
	     *
	     * @param property
	     *            the property for which the {@link org.hibernate.envers.AuditOverride}
	     *            annotations are being processed
	     * @param propertyData
	     *            the Envers auditing data for this property
	     * @return {@code false} if isAudited() of the override annotation was set to
	     */
	    private bool ProcessPropertyAuditingOverrides(PropertyInfo property, PropertyAuditingData propertyData) {
		    // if this property is part of a component, process all override annotations
		    if (this.auditedPropertiesHolder is ComponentAuditingData) {
			    IList<AuditOverrideAttribute> overrides = ((ComponentAuditingData) this.auditedPropertiesHolder).AuditingOverrides;
			    foreach (AuditOverrideAttribute ovr in overrides) {
				    if (property.Name.Equals(ovr.Name)) {
					    // the override applies to this property
					    if (!ovr.IsAudited) {
						    return false;
					    } else {
						    if (ovr.AuditJoinTable != null) {
							    propertyData.JoinTable = ovr.AuditJoinTable;
						    }
					    }
				    }
			    }
    			
		    }
		    return true;
	    }

	    private static AuditJoinTableAttribute DEFAULT_AUDIT_JOIN_TABLE = new DefaultAuditJoinTableAttribute();

        private class ComponentPropertiesSource : IPersistentPropertiesSource {
		    private System.Type xclass;
		    private Component component;

		    public ComponentPropertiesSource(Component component) {
			    try {                    
                    this.xclass = component.ComponentClass;

				    //this.xclass = reflectionManager.classForName(component.getComponentClassName(), this.getClass());
			    } catch (Exception e) {
				    throw new MappingException(e);
			    }

			    this.component = component;
		    }

		    public IEnumerator<Property> PropertyEnumerator { get { return component.PropertyIterator.GetEnumerator(); } }
		    public Property GetProperty(String propertyName) { return component.GetProperty(propertyName); }
		    public System.Type GetClass() { return xclass; }
	    }
    }
}
