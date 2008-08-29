using NHibernateEntityMode = NHibernate.EntityMode;

namespace NHibernate.Annotations
{
	public class EntityModeConverter
	{
		public static NHibernateEntityMode Convert(EntityMode @enum)
		{
			switch(@enum)
			{
				case EntityMode.Poco:
					return NHibernateEntityMode.Poco;

				case EntityMode.DynamicEntity:
					return NHibernateEntityMode.Map;

				default:
					return default(NHibernateEntityMode);
			}
		}
	}
}