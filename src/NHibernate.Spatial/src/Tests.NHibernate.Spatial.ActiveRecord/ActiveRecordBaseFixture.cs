using System;
using NUnit.Framework;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;
using GisSharpBlog.NetTopologySuite.IO;
using NHibernate.Expression;
using NHibernate.Spatial.Expression;
using NHibernate_Spatial_Tests.Model;
using Castle.ActiveRecord;

namespace NHibernate_Spatial_Tests
{
	[TestFixture]
	public class ActiveRecordBaseFixture
	{
		[Test]
		public void FindFirst()
		{
			County county = County.FindFirst();
			IGeometry g = county.Boundaries;
			string wkt = g.AsText();
			double area = g.Area;
		}

		[Test]
		public void FindOneContainsPoint()
		{
			IGeometry queryPoint = new Point(123, 456);
			queryPoint.SRID = 543;
			County county = County.FindOne(
				//SpatialExpression.Contains("Boundaries", new Point(123, 456))
				SpatialExpression.Contains("Boundaries", queryPoint)
				);
		}

		[Test]
		public void FindAllByCriteria()
		{
			IEnvelope envelope = new Envelope(0	, 347500, 0, 10000000);
			IGeometry env = GeometryFactory.Default.ToGeometry(new Envelope(347000, 347500, 0, 10000000));
			env.SRID = 32719;
			County[] counties = County.FindAll(
				  Order.Asc("State")
				, SpatialExpression.Filter("Boundaries", envelope, -1)
				, SpatialExpression.Disjoint("Boundaries", env)
				, SpatialExpression.IsValid("Boundaries")
				, Expression.Not(SpatialExpression.IsEmpty("Boundaries"))
				);
		}

		[Test]
		public void CreateNewCounty()
		{
			long id;
			double area;

			using (new SessionScope())
			{
				County county = new County();
				county.Name = "New county";
				county.State = "Some state";
				county.Boundaries = new Polygon(new LinearRing(new ICoordinate[] { 
					new Coordinate(0,0),
					new Coordinate(1234,0),
					new Coordinate(2345,4321),
					new Coordinate(0,0),
				}));
				area = county.Boundaries.Area;
				county.SaveAndFlush();
				id = county.Id;
			}

			using (new SessionScope())
			{
				County county = County.Find(id);
				Assert.AreEqual(area, county.Boundaries.Area);
				county.Delete();
			}

		}

		[Test]
		public void CreateNewCountyFromWKT()
		{
			long id;
			double area;

			using (new SessionScope())
			{
				County county = new County();
				county.Name = "New county";
				county.State = "Some state";
				county.Boundaries = new WKTReader()
					.Read("POLYGON((0 0, 1234 0, 2345 4321, 0 0))");
				area = county.Boundaries.Area;
				county.SaveAndFlush();
				id = county.Id;
			}

			using (new SessionScope())
			{
				County county = County.Find(id);
				Assert.AreEqual(area, county.Boundaries.Area);
				county.Delete();
			}

		}

	}
}
