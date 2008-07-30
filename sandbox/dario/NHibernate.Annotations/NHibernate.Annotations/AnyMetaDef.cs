using System;

namespace NHibernate.Annotations
{
	//TODO: doc
	public class AnyMetaDefAttribute : Attribute
	{
		public string Name { get; set; }
		public string MetaType { get; set; }
		public string IdType { get; set; }
		public MetaValue[] MetaValues { get; set; }
	}

//* If defined, assign a global meta definition name to be used in an @Any or @ManyToAny annotation
//* If not defined, the metadata applies to the current property or field
//*/
//String name() default "";

///**
//* meta discriminator Hibernate type
//*/
//String metaType();

///**
//* Hibernate type of the id column
//* @return
//*/
//String idType();

///**
//* Matching discriminator values with their respective entity
//*/
//MetaValue[] metaValues();

}