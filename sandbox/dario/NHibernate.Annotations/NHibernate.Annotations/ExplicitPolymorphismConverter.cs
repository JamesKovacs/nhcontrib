using System.Persistence;

namespace NHibernate.Annotations
{
	public class ExplicitPolymorphismConverter
	{
		public static bool Convert(PolymorphismType type)
		{
			switch (type)
			{
				case PolymorphismType.Explicit:
					return true;
				case PolymorphismType.Implicit:
					return true;
				default:
					throw new AnnotationException("Unknown polymorphism type");
			}
		}
	}
}