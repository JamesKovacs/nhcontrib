using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.OgcSfSql11Compliance.Model
{
	[Serializable]
	public class Stream
	{
		public Stream()
		{
		}

		public Stream(long Fid, string Name, IGeometry Centerline)
		{
			this.Fid = Fid;
			this.Name = Name;
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

		private IGeometry _centerline;
		public IGeometry Centerline
		{
			get { return _centerline; }
			set { _centerline = value; }
		}
	}
}
