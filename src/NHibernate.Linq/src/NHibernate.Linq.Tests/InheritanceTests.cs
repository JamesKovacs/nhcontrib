using System.Linq;
using NHibernate.Linq.Tests.Entities;
using NUnit.Framework;

namespace NHibernate.Linq.Tests
{
	[TestFixture]
	public class InheritanceTests : BaseTest
	{
		protected override ISession CreateSession()
		{
			return GlobalSetup.CreateSession();
		}

		[Test]
		public void CanSelectLizardsUsingOfType()
		{
			var lizards = session.Linq<Animal>().OfType<Lizard>().ToArray();
			Assert.AreEqual(2, lizards.Length);
		}

		[Test]
		public void CanSelectMotherOfType()
		{
			var children = (from animal in session.Linq<Animal>()
							where animal.Mother is Mammal
							select animal).ToArray();

			var child = children.Single();
			Assert.AreEqual("5678", child.SerialNumber);
		}
	}
}
