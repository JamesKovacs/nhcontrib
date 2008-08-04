using System;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;

namespace Tests.NHibernate.Spatial.NtsTestCases.Model
{
	[Serializable]
	public class NtsTestCase
	{
		private long _Id;
		public virtual long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}

		private string _Description;
		public virtual string Description
		{
			get { return _Description; }
			set { _Description = value; }
		}

		private IGeometry _GeometryA = GeometryCollection.Empty;
		public virtual IGeometry GeometryA
		{
			get { return this._GeometryA; }
			set { this._GeometryA = value; }
		}

		private IGeometry _GeometryB = GeometryCollection.Empty;
		public virtual IGeometry GeometryB
		{
			get { return this._GeometryB; }
			set { this._GeometryB = value; }
		}

		private string _Operation;
		public virtual string Operation
		{
			get { return _Operation; }
			set { _Operation = value; }
		}

		private string _RelatePattern;
		public virtual string RelatePattern
		{
			get { return _RelatePattern; }
			set { _RelatePattern = value; }
		}

		private IGeometry _GeometryResult = GeometryCollection.Empty;
		public virtual IGeometry GeometryResult
		{
			get { return this._GeometryResult; }
			set { this._GeometryResult = value; }
		}

		private bool _BooleanResult;
		public virtual bool BooleanResult
		{
			get { return this._BooleanResult; }
			set { this._BooleanResult = value; }
		}
	}
}
