using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.OgcSfSql11Compliance.Model
{
	[Serializable]
	public class DividedRoute
	{
		public DividedRoute()
		{
		}

		public DividedRoute(long Fid, string Name, int NumLanes, IGeometry Centerlines)
		{
			this.Fid = Fid;
			this.Name = Name;
			this.NumLanes = NumLanes;
			this.Centerlines = Centerlines;
		}

		private long _fid;
		public long Fid
		{
			get { return _fid; }
			set { _fid = value; }
		}

		private string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private int _numLanes;
		public int NumLanes
		{
			get { return _numLanes; }
			set { _numLanes = value; }
		}

		private IGeometry _centerlines;
		public IGeometry Centerlines
		{
			get { return _centerlines; }
			set { _centerlines = value; }
		}

	}
}
