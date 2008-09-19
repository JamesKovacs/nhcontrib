namespace NHibernate.ProxyGenerators.ActiveRecord
{
	using System;
	using System.Reflection;
	using Castle;
	using Cfg;
	using global::Castle.ActiveRecord;

	[Serializable]
	public class ActiveRecordProxyGenerator : CastleProxyGenerator
	{
		protected override Configuration CreateNHibernateConfiguration( Assembly[] inputAssemblies, ProxyGeneratorOptions options )
		{
			ActiveRecordConfigurationSource activeRecordConfiguration = new ActiveRecordConfigurationSource();
			activeRecordConfiguration.Add(typeof(ActiveRecordBase), GetDefaultNHibernateProperties());

			ActiveRecordStarter.Initialize(inputAssemblies, activeRecordConfiguration);

			Configuration nhibernateConfiguration = ActiveRecordMediator.GetSessionFactoryHolder().GetConfiguration(typeof(ActiveRecordBase));

			foreach(Assembly inputAssembly in inputAssemblies)
			{
				nhibernateConfiguration.AddAssembly(inputAssembly);
			}

			return nhibernateConfiguration;
		}
	}
}