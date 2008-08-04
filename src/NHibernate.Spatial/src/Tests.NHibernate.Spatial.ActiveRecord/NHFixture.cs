using System;
using NUnit.Framework;
using NHibernate;
using NHibernate_Spatial_Tests.Model;
using GeoAPI.Geometries;
using NHibernate.Spatial.Expression;
using NHibernate.Expression;
using System.Collections.Generic;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace NHibernate_Spatial_Tests
{
	[TestFixture]
	public class NHFixture
	{
		private ISession session;

		[SetUp]
		public void SetUp()
		{
			session = null;
		}

		[TearDown]
		public void TearDown()
		{
			session = null;
		}

		[Test]
		public void FindOneContainsPoint()
		{
			IGeometry queryPoint = new Point(123, 456);
			queryPoint.SRID = 543;
			County county = session.CreateCriteria(typeof(County))
				//.Add(SpatialExpression.Contains("Boundaries", new Point(123, 456)))
				.Add(SpatialExpression.Contains("Boundaries", queryPoint))
				.SetFirstResult(1)
				.UniqueResult<County>();
		}

		[Test]
		public void FindAllByCriteria()
		{
			IEnvelope envelope = new Envelope(0, 347500, 0, 10000000);
			IGeometry env = GeometryFactory.Default.ToGeometry(new Envelope(347000, 347500, 0, 10000000));
			env.SRID = 32719;
			IList<County> counties = session.CreateCriteria(typeof(County))
				.AddOrder(Order.Asc("State"))
				.Add(SpatialExpression.Filter("Boundaries", envelope, -1))
				.Add(SpatialExpression.Disjoint("Boundaries", env))
				.Add(SpatialExpression.IsValid("Boundaries"))
				.Add(Expression.Not(SpatialExpression.IsEmpty("Boundaries")))
				.List<County>();
		}
	}
}
