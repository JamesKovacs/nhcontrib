using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.OgcSfSql11Compliance.Model
{
	[Serializable]
	public class MapNeatline
	{
		public MapNeatline()
		{
		}

		public MapNeatline(long Fid, IGeometry Neatline)
		{
			this.Fid = Fid;
			this.Neatline = Neatline;
		}

		private long _fid;
		public long Fid
		{
			get { return _fid; }
			set { _fid = value; }
		}

		private IGeometry _neatline;
		public IGeometry Neatline
		{
			get { return _neatline; }
			set { _neatline = value; }
		}
	}
}
