using System.Linq;
using NHibernate.Linq.Tests.Entities;
using NUnit.Framework;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class SelectManyTests : BaseTest
	{
		protected override ISession CreateSession()
		{
			return GlobalSetup.CreateSession();
		}

		[Test]
		public void CanSelectManyWithAssociationInWhereClause()
		{
			int[] typeCodes = new[] { 5, 15, 100 };

			var query = session.Linq<Patient>().SelectMany(p => p.PatientRecords, (p, r) => r)
				.Where(r => r.Gender == Gender.Male && typeCodes.Contains(r.Type.TypeCode)).ToArray();

			Assert.AreEqual(2, query.Length);
		}
	}
}