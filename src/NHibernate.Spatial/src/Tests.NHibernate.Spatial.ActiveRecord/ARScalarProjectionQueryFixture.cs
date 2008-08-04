using System;
using NUnit.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate_Spatial_Tests.Model;
using NHibernate.Spatial.Expression;
using GeoAPI.Geometries;

namespace NHibernate_Spatial_Tests
{
	[TestFixture]
	public class ARScalarProjectionQueryFixture
	{

		[Test]
		public void QueryCollectAggregate()
		{
			ScalarProjectionQuery<County, IGeometry> query =
				new ScalarProjectionQuery<County, IGeometry>(
					SpatialProjections.Collect("Boundaries"));
			IGeometry envelope = query.Execute();
		}

		[Test]
		public void QueryEnvelopeAggregate()
		{
			ScalarProjectionQuery<County, IGeometry> query =
				new ScalarProjectionQuery<County, IGeometry>(
					SpatialProjections.Envelope("Boundaries"));
			IGeometry envelope = query.Execute();
		}

		[Test]
		public void QueryIntersectionAggregate()
		{
			ScalarProjectionQuery<County, IGeometry> query =
				new ScalarProjectionQuery<County, IGeometry>(
					SpatialProjections.Intersection("Boundaries"));
			IGeometry envelope = query.Execute();
		}

		[Test]
		public void QueryUnionAggregate()
		{
			ScalarProjectionQuery<County, IGeometry> query =
				new ScalarProjectionQuery<County, IGeometry>(
					SpatialProjections.Union("Boundaries"));
			IGeometry envelope = query.Execute();
		}

	}
}
