using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.OgcSfSql11Compliance.Model
{
	[Serializable]
	public class Forest
	{
		public Forest()
		{
		}

		public Forest(long Fid, string Name, IGeometry Boundary)
		{
			this.Fid = Fid;
			this.Name = Name;
			this.Boundary = Boundary;
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

		private IGeometry _boundary;
		public IGeometry Boundary
		{
			get { return _boundary; }
			set { _boundary = value; }
		}
}
}
