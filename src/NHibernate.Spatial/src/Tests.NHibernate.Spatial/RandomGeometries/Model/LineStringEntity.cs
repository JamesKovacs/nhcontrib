using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.RandomGeometries.Model
{
	[Serializable]
	public class LineStringEntity
	{
		public LineStringEntity()
		{
		}

		public LineStringEntity(string Name, IGeometry Geometry)
		{
			this.Name = Name;
			this.Geometry = Geometry;
		}

		public LineStringEntity(long Id, string Name, IGeometry Geometry)
		{
			this.Id = Id;
			this.Name = Name;
			this.Geometry = Geometry;
		}

		private long _Id;
		public virtual long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}

		private string _Name;
		public virtual string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		private IGeometry _Geometry;
		public virtual IGeometry Geometry
		{
			get { return this._Geometry; }
			set { this._Geometry = value; }
		}
	}
}
