using System.Persistence;
using NHibernate.Engine;

namespace NHibernate.Annotations
{
	public class OptimisticLockModeConverter
	{
		public static Versioning.OptimisticLock Convert(OptimisticLockType @enum)
		{
			switch (@enum)
			{
				case OptimisticLockType.All:
					return Versioning.OptimisticLock.All;

				case OptimisticLockType.Dirty:
					return Versioning.OptimisticLock.Dirty;

				case OptimisticLockType.None:
					return Versioning.OptimisticLock.None;

				case OptimisticLockType.Version:
					return Versioning.OptimisticLock.Version;

				default:
					return default(Versioning.OptimisticLock);
			}
		}
	}
}