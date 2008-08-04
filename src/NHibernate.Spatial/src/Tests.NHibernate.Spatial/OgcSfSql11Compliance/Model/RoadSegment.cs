using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.OgcSfSql11Compliance.Model
{
	[Serializable]
	public class RoadSegment
	{
		public RoadSegment()
		{
		}

		public RoadSegment(long Fid, string Name, string Aliases, int NumLanes, IGeometry Centerline)
		{
			this.Fid = Fid;
			this.Name = Name;
			this.Aliases = Aliases;
			this.NumLanes = NumLanes;
			this.Centerline = Centerline;
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

		private string _aliases;
		public string Aliases
		{
			get { return _aliases; }
			set { _aliases = value; }
		}

		private int _numLanes;
		public int NumLanes
		{
			get { return _numLanes; }
			set { _numLanes = value; }
		}

		private IGeometry _centerline;
		public IGeometry Centerline
		{
			get { return _centerline; }
			set { _centerline = value; }
		}
	}
}
