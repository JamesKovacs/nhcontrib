namespace NHibernate.ProxyGenerators
{
	using System.Reflection;

	public interface IProxyGenerator
	{
		Assembly Generate(ProxyGeneratorOptions proxyGeneratorOptions);

		ProxyGeneratorOptions GetOptions(); 
	}
}