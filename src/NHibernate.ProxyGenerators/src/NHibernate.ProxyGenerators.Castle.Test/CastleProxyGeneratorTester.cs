namespace NHibernate.ProxyGenerators.Castle.Test
{
	using System;
	using ProxyGenerators.Test;
	using NUnit.Framework;

	[TestFixture]
	[Serializable]
	public class CastleProxyGeneratorTester : ProxyGeneratorTester
	{
		protected override IProxyGenerator CreateGenerator()
		{
			return new CaslteProxyGenerator();
		}
	}
}