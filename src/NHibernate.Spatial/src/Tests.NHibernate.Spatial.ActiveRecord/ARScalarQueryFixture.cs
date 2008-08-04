using System;
using NUnit.Framework;
using GeoAPI.Geometries;
using Castle.ActiveRecord.Queries;
using NHibernate_Spatial_Tests.Model;

namespace NHibernate_Spatial_Tests
{
	[TestFixture]
	public class ARScalarQueryFixture
	{
		[Test]
		public void QueryBoundariesConvexHull()
		{
			// For projections, it is necessary to register functions (eg. ST.ConvexHull)
			ScalarQuery<IGeometry> sqg = new ScalarQuery<IGeometry>(typeof(County), @"
				select NHS.ConvexHull(c.Boundaries)
				from County c
				");
			sqg.SetQueryRange(1);
			IGeometry g = sqg.Execute();
		}

		[Test]
		public void QueryBoundariesArea()
		{
			ScalarQuery<double> sqd = new ScalarQuery<double>(typeof(County), @"
				select NHS.Area(c.Boundaries)
				from County c
				");
			sqd.SetQueryRange(1);
			double d = sqd.Execute();
		}

		[Test]
		public void QueryBoundariesExteriorRing()
		{
			ScalarQuery<IGeometry> sqg = new ScalarQuery<IGeometry>(typeof(County), @"
				select NHS.ExteriorRing(c.Boundaries)
				from County c
				");
			sqg.SetQueryRange(1);
			IGeometry g = sqg.Execute();
		}

		[Test]
		public void QueryBoundariesExteriorRingCastedDirectlyToLineString()
		{
			ScalarQuery<ILineString> sqls = new ScalarQuery<ILineString>(typeof(County), @"
				select NHS.ExteriorRing(c.Boundaries)
				");
			sqls.SetQueryRange(1);
			ILineString ls = sqls.Execute();
		}
	}
}
