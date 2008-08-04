using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.OgcSfSql11Compliance.Model
{
	[Serializable]
	public class Lake
	{
		public Lake()
		{
		}

		public Lake(long Fid, string Name, IGeometry Shore)
		{
			this.Fid = Fid;
			this.Name = Name;
			this.Shore = Shore;
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

		private IGeometry _shore;
		public IGeometry Shore
		{
			get { return _shore; }
			set { _shore = value; }
		}
	}
}
