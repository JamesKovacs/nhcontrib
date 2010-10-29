namespace NHibernate.Envers.Tests.Entities.Components
{
	public class ComponentTestEntity
	{
		public virtual int Id { get; set; }
		[Audited]
		public virtual Component1 Comp1 { get; set; }
		public virtual Component2 Comp2 { get; set; }
	}
}