using System.Collections;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using log4net;
using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Annotations.Cfg
{
	public class AnnotationConfiguration : Configuration
	{
		private static readonly ILog log = LogManager.GetLogger(typeof (AnnotationConfiguration));

		public static readonly string ARTEFACT = "hibernate.mapping.precedence";
		public static readonly string DEFAULT_PRECEDENCE = "hbm, class";
		private Dictionary<string, System.Type> annotatedClassEntities;
		private List<System.Type> annotatedClasses;
		private Dictionary<string, AnyMetaDefAttribute> anyMetaDefs;
		//private List<ICacheHolder> caches;
		private Dictionary<string, AnnotatedClassType> classTypes;
		private ISet<string> defaultNamedGenerators;
		private ISet<string> defaultNamedNativeQueryNames;
		private ISet<string> defaultNamedQueryNames;
		private ISet<string> defaultSqlResulSetMappingNames;
		//private Dictionary<string, Properties> generatorTables;
		//private List<Document> hbmDocuments; //user ordering matters, hence the list
		//private Dictionary<string, Document> hbmEntities;
		private bool inSecondPass;
		private bool isDefaultProcessed;
		private bool isValidatorNotPresentLogged;
		private Dictionary<string, Dictionary<string, Join>> joins;
		private Dictionary<string, string> mappedByResolver;
		private IDictionary namedGenerators;
		private string precedence;
		private Dictionary<string, string> propertyRefResolver;
		private Dictionary<Table, List<string[]>> tableUniqueConstraints;

		protected internal AnnotationConfiguration(SettingsFactory settingsFactory)
			: base(settingsFactory)
		{
		}

		public AnnotationConfiguration()
		{
		}

		public AnnotationConfiguration AddAnnotatedClass(System.Type persistentClass)
		{
			//XClass persistentXClass = reflectionManager.toXClass(persistentClass );
			try
			{
				annotatedClasses.Add(persistentClass);
				return this;
			}
			catch (MappingException me)
			{
				log.Error("Could not compile the mapping annotations", me);
				throw me;
			}
		}
	}
}