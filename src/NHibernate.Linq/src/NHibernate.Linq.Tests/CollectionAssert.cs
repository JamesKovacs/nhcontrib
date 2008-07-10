namespace NHibernate.Linq.Tests
{
	using System.Collections;
	using NUnit.Framework;
	using System.Linq;

	public class CollectionAssert
	{
		public static void AreCountEqual(int expectedCount, IEnumerable collection)
		{
			Assert.AreEqual(expectedCount, collection.Cast<object>().Count());
		}
	}
}