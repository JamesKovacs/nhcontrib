using System.Persistence;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Cfg
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
		ColumnAttribute[] GetOverriddenColumn(string propertyName);

		/// <summary>
		/// return null if the column is not overridden, or an array of column if true
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		JoinColumnAttribute[] GetOverriddenJoinColumn(string propertyName);

		void AddProperty(Property prop, Ejb3Column[] columns);

		Join AddJoin(JoinTableAttribute joinTableAnn, bool noDelayInPkColumnCreation);
	}
}