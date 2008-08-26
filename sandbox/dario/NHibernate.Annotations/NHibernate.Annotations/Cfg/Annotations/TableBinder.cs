using NHibernate.Annotations.NHibernate.Cfg;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Annotations.Cfg.Annotations
{
	public class TableBinder
	{
		public static void BindFk(PersistentClass @ref, object o, Ejb3JoinColumn[] columns, ManyToOne one, bool unique, ExtendedMappings mappings)
		{
			throw new System.NotImplementedException();
		}
	}
}