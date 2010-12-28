namespace NHibernate.Envers.Tests.Integration.Inheritance.Joined.Relation
{
	[Audited]
	public class ParentIngEntity
	{
		public virtual int Id { get; set; }
		public virtual string Data { get; set; }
		public virtual ReferencedEntity Referenced { get; set; }

		public override bool Equals(object obj)
		{
			var casted = obj as ParentIngEntity;
			if (casted == null)
				return false;
			return (Id == casted.Id && Data == casted.Data);
		}

		public override int GetHashCode()
		{
			return Id ^ Data.GetHashCode();
		}
	}
}