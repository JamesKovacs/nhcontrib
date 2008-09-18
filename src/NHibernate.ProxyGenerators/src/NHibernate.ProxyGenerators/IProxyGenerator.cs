namespace NHibernate.ProxyGenerators
{
	using System.Reflection;

	public interface IProxyGenerator
	{
		Assembly Generate(string outputAssemblyPath, params Assembly[] inputAssemblies);

		ProxyGeneratorOptions GetOptions(); 
	}
}