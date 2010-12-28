using Iesi.Collections.Generic;

namespace NHibernate.Envers.Tests.Integration.Inheritance.Joined.Relation
{
	[Audited]
	public class ReferencedEntity
	{
		public virtual int Id { get; set; }
		public virtual ISet<ParentIngEntity> Referencing { get; set; }

		public override bool Equals(object obj)
		{
			var casted = obj as ReferencedEntity;
			if (casted == null)
				return false;
			return (Id == casted.Id);
		}

		public override int GetHashCode()
		{
			return Id;
		}
	}
}