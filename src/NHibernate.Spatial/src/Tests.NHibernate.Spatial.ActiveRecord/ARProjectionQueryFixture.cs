using System;
using NUnit.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate.Spatial.Expression;
using NHibernate.Expression;
using NHibernate;
using NHibernate.Type;
using NHibernate.Spatial.Type;
using System.Collections.Generic;
using NHibernate_Spatial_Tests.Model;

namespace NHibernate_Spatial_Tests
{
	[TestFixture]
	public class ARProjectionQueryFixture
	{
		[Test]
		public void QueryGroupedByState()
		{
			ProjectionQuery<County> proj = new ProjectionQuery<County>(
				Projections.ProjectionList()
				.Add(Projections.GroupProperty("State"))
				.Add(Projections.RowCount())
				.Add(SpatialProjections.Envelope("Boundaries"))
				.Add(SpatialProjections.Collect("Boundaries"))
				);
			IList<object[]> lo = proj.Execute();
		}

		[Test]
		public void QueryCoordinateTransformation()
		{
			ProjectionQuery<County> proj = new ProjectionQuery<County>(
				Projections.ProjectionList()
				.Add(Projections.Property("Name"))
				.Add(Projections.Property("Boundaries"))
				.Add(SpatialProjections.Transform("Boundaries", 32718))
				.Add(SpatialProjections.Transform("Boundaries", 32719))
				);
			IList<object[]> lo = proj.Execute();
		}

	}
}
