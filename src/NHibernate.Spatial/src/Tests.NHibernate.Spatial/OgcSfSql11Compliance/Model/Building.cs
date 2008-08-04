using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.OgcSfSql11Compliance.Model
{
	[Serializable]
	public class Building
	{
		public Building()
		{
		}

		public Building(long Fid, string Address, IGeometry Position, IGeometry Footprint)
		{
			this.Fid = Fid;
			this.Address = Address;
			this.Position = Position;
			this.Footprint = Footprint;
		}

		private long _fid;
		public long Fid
		{
			get { return _fid; }
			set { _fid = value; }
		}

		private string _address;
		public string Address
		{
			get { return _address; }
			set { _address = value; }
		}

		private IGeometry _position;
		public IGeometry Position
		{
			get { return _position; }
			set { _position = value; }
		}

		private IGeometry _Footprint;
		public IGeometry Footprint
		{
			get { return _Footprint; }
			set { _Footprint = value; }
		}
	}
}
