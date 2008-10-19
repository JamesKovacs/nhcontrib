namespace NHibernate.ProxyGenerators.Castle
{
	using Bytecode;
	using Proxy;

	public class CastleProxyFactoryFactory : IProxyFactoryFactory
	{
		private static IProxyFactory _proxyFactory;

		public static IProxyFactory ProxyFactory
		{
			get { return _proxyFactory; }
			set { _proxyFactory = value; }
		}

		public IProxyFactory BuildProxyFactory()
		{
			return ProxyFactory;
		}

		public IProxyValidator ProxyValidator
		{
			get { return new DynProxyTypeValidator(); }
		}
	}
}