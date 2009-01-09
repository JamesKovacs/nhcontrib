using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Metadata;

namespace NHibernate.Linq.Util
{
	public static class SessionFactoryUtil
	{
		public static IDictionary<System.Type, System.Type> GetProxyMetaData(this ISessionFactoryImplementor factory, IDictionary metaData)
		{
			var dict = new Dictionary<System.Type, System.Type>();
			foreach (System.Type key in metaData.Keys)
			{
				var item = (IClassMetadata)metaData[key];
				if (item.HasProxy)
				{
					var proxyType = factory.GetEntityPersister(key).GetConcreteProxyClass(EntityMode.Poco);
					if (proxyType != item.GetMappedClass(EntityMode.Poco))
					{
						dict.Add(proxyType, key);
					}
				}
			}
			return dict;
		}

		public static IDictionary<string, System.Type> GetEntityNameMetaData(this ISessionFactoryImplementor factory)
		{
			var metaData = factory.GetAllClassMetadata();

			var dict = new Dictionary<string, System.Type>();
			foreach (System.Type type in metaData.Keys)
			{
				var item = (IClassMetadata)metaData[type];

				dict.Add(item.EntityName, type);
				if (item.HasProxy)
				{
					var proxyType = factory.GetEntityPersister(type).GetConcreteProxyClass(EntityMode.Poco);
					if (proxyType != type)
					{
						dict.Add(proxyType.FullName, proxyType);
					}
				}
			}
			return dict;
		}
	}
}
