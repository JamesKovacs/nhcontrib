using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.OgcSfSql11Compliance.Model
{
	[Serializable]
	public class Bridge
	{
		public Bridge()
		{
		}

		public Bridge(long Fid, string Name, IGeometry Position)
		{
			this.Fid = Fid;
			this.Name = Name;
			this.Position = Position;
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

		private IGeometry _position;
		public IGeometry Position
		{
			get { return _position; }
			set { _position = value; }
		}
	}
}
