using System;

namespace NHibernate.Annotations.Cfg.Annotations
{
	/// <summary>
	/// Specify a custom persister.
	/// </summary>
	public class PersisterAttribute : Attribute
	{
		public System.Type Implementation { get; set; }
	}
}