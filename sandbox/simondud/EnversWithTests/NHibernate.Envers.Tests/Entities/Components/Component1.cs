namespace NHibernate.Envers.Tests.Entities.Components
{
	public class Component1
	{
		public virtual string Str1 { get; set; }
		public virtual string Str2 { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as Component1;
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			return Str1.Equals(other.Str1) && Str2.Equals(other.Str2);
		}

		public override int GetHashCode()
		{
			return Str1.GetHashCode() ^ Str2.GetHashCode();
		}
	}
}