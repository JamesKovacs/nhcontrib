using System;
using System.Collections;
using System.Reflection;
using System.Security;
using Iesi.Collections.Generic;
using log4net;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Engine;
using NHibernate.Proxy;
using NHibernate.ProxyGenerators.CastleDynamicProxy;
using NHibernate.Type;
using IInterceptor = Castle.Core.Interceptor.IInterceptor;

//[assembly: AssemblyVersion("{VERSION}")]
//[assembly: AllowPartiallyTrustedCallers]

public class CastleStaticProxyFactory : IProxyFactory
{
	private static readonly ILog _log;
	private static readonly IDictionary _proxies;

	private string _entityName;
	private Type _persistentClass;
	private Type[] _interfaces;
	private MethodInfo _getIdentifierMethod;
	private MethodInfo _setIdentifierMethod;
	private IAbstractComponentType _componentIdType;
	private bool _isClassProxy;
	private string _proxyKey;
	private Type _proxyType;

	static CastleStaticProxyFactory()
	{
		_log = LogManager.GetLogger(typeof(CastleStaticProxyFactory));
		_proxies = new Hashtable();
		//{PROXIES}
	}

	public void PostInstantiate(string entityName, Type persistentClass, ISet<Type> interfaces, MethodInfo getIdentifierMethod, MethodInfo setIdentifierMethod, IAbstractComponentType componentIdType)
	{
		_entityName = entityName;
		_persistentClass = persistentClass;
		_interfaces = new Type[interfaces.Count];
		interfaces.CopyTo(_interfaces, 0);
		_getIdentifierMethod = getIdentifierMethod;
		_setIdentifierMethod = setIdentifierMethod;
		_componentIdType = componentIdType;
		_isClassProxy = _interfaces.Length == 1;

		_proxyKey = entityName;

		if( _proxies.Contains(_proxyKey) )
		{
			_proxyType = _proxies[_proxyKey] as Type;
			_log.DebugFormat("Using proxy type '{0}' for persistent class '{1}'", _proxyType.Name, _persistentClass.FullName);
		}
		else
		{
			string message = string.Format("No proxy type found for persistent class '{0}' using proxy key '{1}'", _persistentClass.FullName, _proxyKey);
			_log.Error(message);
			throw new HibernateException(message);
		}
	}

	public INHibernateProxy GetProxy(object id, ISessionImplementor session)
	{
		INHibernateProxy proxy;
		try
		{
			object generatedProxy;
			LazyInitializer initializer = new LazyInitializer(_entityName, _persistentClass, id, _getIdentifierMethod, _setIdentifierMethod, _componentIdType, session);
			IInterceptor[] interceptors = new IInterceptor[] { initializer };

			object[] args;
			if (_isClassProxy)
			{
				args = new object[] { interceptors };
			}
			else
			{
				args = new object[] { interceptors, new object() };
			}
			generatedProxy = Activator.CreateInstance(_proxyType, args);

			initializer._constructed = true;
			proxy = (INHibernateProxy)generatedProxy;
		}
		catch (Exception e)
		{
			string message = "Creating a proxy instance failed";
			_log.Error(message, e);
			throw new HibernateException(message, e);
		}

		return proxy;
	}
}

public class CastleStaticProxyFactoryFactory : IProxyFactoryFactory
{
	public IProxyFactory BuildProxyFactory()
	{
		return new CastleStaticProxyFactory();
	}

	public IProxyValidator ProxyValidator
	{
		get { return new DynProxyTypeValidator(); }
	}
}