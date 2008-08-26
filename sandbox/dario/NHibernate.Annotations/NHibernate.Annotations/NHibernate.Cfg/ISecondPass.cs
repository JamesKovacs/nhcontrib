using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Annotations.NHibernate.Cfg
{
	/// <summary>
	/// This interface should be on NHibernate core, instead that are using delegates to 
	/// execute Second Pass commands.
	/// <seealso cref="SecondPassCommand"/>
	/// </summary>
	public interface ISecondPass
	{
		void DoSecondPass(IDictionary<string, PersistentClass> persistentClasses);
	}
}
