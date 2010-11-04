namespace NHibernate.Envers.Tests.Entities.Components.Relations
{
	public class ManyToOneComponentTestEntity
	{
		public virtual int Id { get; set; }
		[Audited]
		public virtual ManyToOneComponent Comp1 { get; set; }
	}
}