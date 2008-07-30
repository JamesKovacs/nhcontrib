namespace NHibernate.Annotations
{
	public interface MetaValue
	{
		System.Type TargetType { get; set; }
		string Value { get; set; }
	}
}