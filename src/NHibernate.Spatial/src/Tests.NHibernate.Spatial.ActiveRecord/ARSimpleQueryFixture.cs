using System;
using NUnit.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate_Spatial_Tests.Model;

namespace NHibernate_Spatial_Tests
{
	[TestFixture]
	public class ARSimpleQueryFixture
	{
		[Test]
		public void QueryCountiesByArea()
		{
			// In the "where", it is not mandatory to use registered functions 
			// but the proper HQL way is strongly recommended in order to get
			// compatibility for multiple spatial dialects.

			SimpleQuery<County> query;
			County[] counties;

			// The following it works (in MsSqlSpatial only):
			query = new SimpleQuery<County>(@"
				from County c
				where ST.Area(c.Boundaries) > 200000
				");
			counties = query.Execute();

			// So, prefer this:
			query = new SimpleQuery<County>(@"
				from County c
				where NHS.Area(c.Boundaries) > 200000
				");
			counties = query.Execute();

			// ("NHS" is just the acronym of NHibernate.Spatial)
		}

	}
}
