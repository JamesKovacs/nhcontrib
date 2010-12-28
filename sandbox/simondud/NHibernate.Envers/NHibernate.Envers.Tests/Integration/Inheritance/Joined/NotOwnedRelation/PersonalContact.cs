namespace NHibernate.Envers.Tests.Integration.Inheritance.Joined.NotOwnedRelation
{
	public class PersonalContact : Contact
	{
		public virtual string FirstName { get; set; }
	}
}