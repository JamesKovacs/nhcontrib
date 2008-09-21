namespace NHibernate.ProxyGenerators.ActiveRecord.Test
{
	using System;
	using System.Reflection;
	using Castle;
	using global::Castle.ActiveRecord;
	using Proxy;
	using ProxyGenerators.Test;
	using NUnit.Framework;

	[TestFixture]
	[Serializable]
	public class ActiveRecordProxyGeneratorTester : ProxyGeneratorTester
	{
		protected override IProxyGenerator CreateGenerator()
		{
			return new ActiveRecordProxyGenerator();
		}

		protected override ProxyGeneratorOptions CreateOptions(string outputAssemblyPath, params string[] inputAssemblyPaths)
		{
			return new CastleProxyGeneratorOptions(outputAssemblyPath, inputAssemblyPaths);
		}

		[Test]
		public void Generates_Proxies_For_ActiveRecords()
		{
			CrossAppDomainCaller.RunInOtherAppDomain(delegate
			{
				Type arUserType = typeof(ARUser);
				Assembly proxyAssembly = _generator.Generate(CreateOptions(_outputAssemblyPath, arUserType.Assembly.Location));

				Assert.IsNotNull(proxyAssembly);

				Type arUserTypeProxy = null;
				foreach (Type type in proxyAssembly.GetTypes())
				{
					if (type.BaseType == arUserType)
					{
						arUserTypeProxy = type;
						break;
					}
				}
				Assert.IsNotNull(arUserTypeProxy);

				Assert.IsTrue(typeof(INHibernateProxy).IsAssignableFrom(arUserTypeProxy));
			});
		}
	}

	[ActiveRecord]
	public class ARUser
	{
		private int _id; 

		[PrimaryKey]
		public virtual int Id
		{
			get { return _id; }
			set { _id = value; }
		}
	}
}