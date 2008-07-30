using System.Persistence;
using NHibernate.Cfg;
using NHibernate.Mapping;
using Column=NHibernate.Mapping.Column;

namespace NHibernate.Annotations.NHibernate.Cfg
{
	public interface PropertyHolder
	{
		string ClassName { get; }

		string EntityOwnerClassName { get; }

		Table Table { get; }
		bool IsComponent { get; }
		bool IsEntity { get; }
		string Path { get; }
		string EntityName { get; }

		void AddProperty(Property prop);

		IKeyValue Identifier{ get; }

		PersistentClass getPersistentClass();

		void SetParentProperty(string parentProperty);

		/// <summary>
		/// return null if the column is not overridden, or an array of column if true
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		Column[] getOverriddenColumn(string propertyName);

		/// <summary>
		/// return null if the column is not overridden, or an array of column if true
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		JoinColumn[] getOverriddenJoinColumn(string propertyName);

		void AddProperty(Property prop, Ejb3Column[] columns);

		Join AddJoin(JoinTable joinTableAnn, bool noDelayInPkColumnCreation);
	}
}