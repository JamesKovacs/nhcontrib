namespace NHibernate.ProxyGenerators.Castle
{
	using System;
	using System.Collections;
	using System.Reflection;
	using Engine;
	using global::Castle.DynamicProxy;
	using Iesi.Collections.Generic;
	using Proxy;
	using Type;

	public class CastleProxyFactory : IProxyFactory
	{
		private readonly IProxyBuilder _proxyBuilder;
		private readonly IDictionary _proxies;

		public CastleProxyFactory(IProxyBuilder proxyBuilder, IDictionary proxies)
		{
			if (proxyBuilder == null)
			{
				throw new ArgumentNullException("proxyBuilder");
			}
			if (proxies == null)
			{
				throw new ArgumentNullException("proxies");
			}

			_proxyBuilder = proxyBuilder;
			_proxies = proxies;
		}

		public void PostInstantiate(string entityName, Type persistentClass, ISet<Type> interfaces, MethodInfo getIdentifierMethod, MethodInfo setIdentifierMethod, IAbstractComponentType componentIdType)
		{
			if (persistentClass.IsGenericType) return;

			int interfacesCount = interfaces.Count;
			bool isClassProxy = interfacesCount == 1;
			Type[] ifaces = new Type[interfacesCount];
			interfaces.CopyTo(ifaces, 0);

			Type proxyType;
			if (isClassProxy)
			{
				proxyType = _proxyBuilder.CreateClassProxy(persistentClass, ifaces, ProxyGenerationOptions.Default);
			}
			else
			{
				proxyType = _proxyBuilder.CreateInterfaceProxyTypeWithoutTarget(ifaces[0], ifaces, ProxyGenerationOptions.Default);
			}

			_proxies[entityName] = proxyType;
		}

		public INHibernateProxy GetProxy(object id, ISessionImplementor session)
		{
			throw new NotImplementedException();
		}
	}
}