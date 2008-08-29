using System;

namespace NHibernate.Annotations
{
	public class LoaderAttribute : Attribute
	{
		public String NamedQuery { get; set; }
	}
}