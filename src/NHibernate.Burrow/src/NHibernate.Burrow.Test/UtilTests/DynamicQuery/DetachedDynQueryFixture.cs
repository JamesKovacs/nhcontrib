using NUnit.Framework;
using NHibernate.Burrow.Util.DynQuery;
using NHibernate.Burrow.Util.NH.Impl;

namespace NHibernate.Burrow.Test.DynamicQuery
{
	[TestFixture]
	public class DetachedDynQueryFixture
	{
		// DetachedDynQuery don't need a test for everything because it is based on the same class of
		// DetachedQuery and use a DynQuery (see the tests of these classes)

		[Test]
		public void ToRowCount()
		{
			Select s = new Select("f.Name, f.Description, b.Descriptoin").From("Foo f join f.Bar b");
			Where where = new Where();
			where.And("f.Name like :pName");
			where.And("b.Asociated > :pAso");
			s.From().SetWhere(where);
			OrderBy order = new OrderBy().Add("b.Asociated", true);
			order.Add("f.Name");
			s.From().SetOrderBy(order);

			DetachedDynQuery ddq = new DetachedDynQuery(s);
			DetachedQuery drc = (DetachedQuery)ddq.TransformToRowCount();
			Assert.AreEqual("select count(*) from Foo f join f.Bar b where ((f.Name like :pName) and (b.Asociated > :pAso))",
			                drc.Hql);

		}
	}
}
