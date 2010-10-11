using System;
using System.Collections.Generic;
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
	    private readonly ModificationStore _defaultStore;
	    private readonly IPersistentPropertiesSource _persistentPropertiesSource;
	    private readonly IAuditedPropertiesHolder _auditedPropertiesHolder;
	    private readonly GlobalConfiguration _globalCfg;
	    private readonly String _propertyNamePrefix;

	    private readonly ISet<String> _propertyAccessedPersistentProperties;
	    private readonly ISet<String> _fieldAccessedPersistentProperties;

	    public AuditedPropertiesReader(ModificationStore defaultStore,
								       IPersistentPropertiesSource persistentPropertiesSource,
								       IAuditedPropertiesHolder auditedPropertiesHolder,
								       GlobalConfiguration globalCfg,
								       String propertyNamePrefix) {
		    _defaultStore = defaultStore;
		    _persistentPropertiesSource = persistentPropertiesSource;
		    _auditedPropertiesHolder = auditedPropertiesHolder;
		    _globalCfg = globalCfg;
		    _propertyNamePrefix = propertyNamePrefix;

		    _propertyAccessedPersistentProperties = Toolz.NewHashSet<String>();
		    _fieldAccessedPersistentProperties = Toolz.NewHashSet<String>();
	    }

	    public void read() {
		    // First reading the access types for the persistent properties.
		    ReadPersistentPropertiesAccess();

		    // Adding all properties from the given class.
		    AddPropertiesFromClass(_persistentPropertiesSource.GetClass());
	    }

	    private void ReadPersistentPropertiesAccess() {
		    IEnumerator<Property> propertyIter = _persistentPropertiesSource.PropertyEnumerator;
		    while (propertyIter.MoveNext()) {
			    var property = propertyIter.Current;
                if ("field".Equals(property.PropertyAccessorName))
                {
                    _fieldAccessedPersistentProperties.Add(property.Name);
                }
                else
                {
                    _propertyAccessedPersistentProperties.Add(property.Name);
                }
		    }
	    }

	    private void AddPropertiesFromClass(System.Type clazz)  {
            //No need to go to base class, the .NET GetProperty method can bring the base properties also
            //System.Type superclazz = clazz.BaseType;
            //if (!"System.Object".Equals(superclazz.FullName))
            //{
            //    AddPropertiesFromClass(superclazz);
            //}

            //ORIG: addFromProperties(clazz.getDeclaredProperties("field"), "field", fieldAccessedPersistentProperties);
            //addFromProperties(clazz.getDeclaredProperties("property"), "property", _propertyAccessedPersistentProperties);
            AddFromProperties(clazz.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public), "field", _fieldAccessedPersistentProperties);
            AddFromProperties(clazz.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public), "property", _propertyAccessedPersistentProperties);
	    }

	    private void AddFromProperties(IEnumerable<MemberInfo> properties, String accessType, ISet<String> persistentProperties) {
		    //ORIG: foreach (XProperty property in properties) {
			foreach (var property in properties) 
            {
			    // If this is not a persistent property, with the same access type as currently checked,
			    // it's not audited as well.
			    if (persistentProperties.Contains(property.Name)) 
                {
				    IValue propertyValue = _persistentPropertiesSource.GetProperty(property.Name).Value;

				    PropertyAuditingData propertyData;
				    bool isAudited;
				    if (propertyValue is Component) 
                    {
					    ComponentAuditingData componentData = new ComponentAuditingData();
					    isAudited = FillPropertyData(property, componentData, accessType);

					    IPersistentPropertiesSource componentPropertiesSource = new ComponentPropertiesSource(
							    (Component) propertyValue);
					    new AuditedPropertiesReader(ModificationStore.FULL, componentPropertiesSource, componentData,
							    _globalCfg,
							    _propertyNamePrefix + MappingTools.createComponentPrefix(property.Name))
							    .read();

					    propertyData = componentData;
				    } else {
					    propertyData = new PropertyAuditingData();
					    isAudited = FillPropertyData(property, propertyData, accessType);
				    }

				    if (isAudited) {
					    // Now we know that the property is audited
					    _auditedPropertiesHolder.addPropertyAuditingData(property.Name, propertyData);
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
	    private bool FillPropertyData(MemberInfo property, PropertyAuditingData propertyData,
									     String accessType) {

		    // check if a property is declared as not audited to exclude it
		    // useful if a class is audited but some properties should be excluded
            var unVer = (NotAuditedAttribute)Attribute.GetCustomAttribute(property, typeof(NotAuditedAttribute));
		    if (unVer != null) 
            {
			    return false;
		    }
	        // if the optimistic locking field has to be unversioned and the current property
	        // is the optimistic locking field, don't audit it
	        if (_globalCfg.isDoNotAuditOptimisticLockingField()) 
            {
	            //Version jpaVer = property.getAnnotation(typeof(Version));
	            var jpaVer = (VersionAttribute)Attribute.GetCustomAttribute(property, typeof(VersionAttribute));
	            if (jpaVer != null) {
	                return false;
	            }
	        }

	        // Checking if this property is explicitly audited or if all properties are.
		    //AuditedAttribute aud = property.getAnnotation(typeof(AuditedAttribute));
            var aud = (AuditedAttribute)Attribute.GetCustomAttribute(property, typeof(AuditedAttribute));
		    if (aud != null) {
			    propertyData.Store = aud.ModStore;
			    propertyData.setRelationTargetAuditMode(aud.TargetAuditMode);
		    } else {
			    if (_defaultStore != ModificationStore._NULL) {
				    propertyData.Store = _defaultStore;
			    } else {
				    return false;
			    }
		    }

		    propertyData.Name = _propertyNamePrefix + property.Name;
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

        private void SetPropertyAuditMappedBy(MemberInfo property, PropertyAuditingData propertyData) 
        {
            var auditMappedBy = (AuditMappedByAttribute)Attribute.GetCustomAttribute(property, typeof(AuditMappedByAttribute));
            if (auditMappedBy != null) 
            {
		        propertyData.AuditMappedBy = auditMappedBy.MappedBy;
                if (!"".Equals(auditMappedBy.PositionMappedBy)) {
                    propertyData.PositionMappedBy = auditMappedBy.PositionMappedBy;
                }
            }
        }

        private void AddPropertyMapKey(MemberInfo property, PropertyAuditingData propertyData) 
        {
		    var mapKey = (MapKeyAttribute)Attribute.GetCustomAttribute(property, typeof(MapKeyAttribute));
		    if (mapKey != null) 
            {
			    propertyData.MapKey = mapKey.Name;
		    }
	    }

	    private void AddPropertyJoinTables(MemberInfo property, PropertyAuditingData propertyData)
        {
		    // first set the join table based on the AuditJoinTable annotation
		    var joinTable = (AuditJoinTableAttribute)Attribute.GetCustomAttribute(property, typeof(AuditJoinTableAttribute));;
		    propertyData.JoinTable = joinTable ?? DEFAULT_AUDIT_JOIN_TABLE;
	    }

	    /***
	     * Add the {@link org.hibernate.envers.AuditOverride} annotations.
	     *
	     * @param property the property being processed
	     * @param propertyData the Envers auditing data for this property
	     */
        private void AddPropertyAuditingOverrides(MemberInfo property, PropertyAuditingData propertyData)
        {
		    var annotationOverride = (AuditOverrideAttribute)Attribute.GetCustomAttribute(property, typeof(AuditOverrideAttribute));
		    if (annotationOverride != null) 
            {
			    propertyData.addAuditingOverride(annotationOverride);
		    }
		    var annotationOverrides = (AuditOverridesAttribute)Attribute.GetCustomAttribute(property, typeof(AuditOverridesAttribute));
		    if (annotationOverrides != null) 
            {
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
        private bool ProcessPropertyAuditingOverrides(MemberInfo property, PropertyAuditingData propertyData) 
        {
		    // if this property is part of a component, process all override annotations
		    if (_auditedPropertiesHolder is ComponentAuditingData) {
			    var overrides = ((ComponentAuditingData) _auditedPropertiesHolder).AuditingOverrides;
			    foreach (var ovr in overrides) 
                {
				    if (property.Name.Equals(ovr.Name))
				    {
				        // the override applies to this property
					    if (!ovr.IsAudited) 
                        {
						    return false;
					    }
				        if (ovr.AuditJoinTable != null) 
                        {
				            propertyData.JoinTable = ovr.AuditJoinTable;
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
