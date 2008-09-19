namespace NHibernate.ProxyGenerators.ActiveRecord
{
	using global::Castle.ActiveRecord.Framework.Config;

	public class ActiveRecordConfigurationSource : InPlaceConfigurationSource
	{
		public ActiveRecordConfigurationSource()
		{
			SetIsLazyByDefault(true);
		}
	}
}