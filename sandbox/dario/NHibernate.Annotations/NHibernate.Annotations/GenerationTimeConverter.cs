using NHibernate.Mapping;

namespace NHibernate.Annotations
{
	/// <summary>
	/// TODO: This should be refactoring and just 1 Enum should exist, but later.
	/// </summary>
	internal class GenerationTimeConverter
	{
		public static PropertyGeneration Convert(GenerationTime @enum)
		{
			switch (@enum)
			{
				case GenerationTime.Always:
					return PropertyGeneration.Always;
				case GenerationTime.Insert:
					return PropertyGeneration.Insert;
				case GenerationTime.Never:
					return PropertyGeneration.Never;
				default:
					return default(PropertyGeneration);
			}
		}
	}
}