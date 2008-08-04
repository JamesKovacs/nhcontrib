using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.OgcSfSql11Compliance.Model
{
	[Serializable]
	public class Pond
	{
		public Pond()
		{
		}

		public Pond(long Fid, string Name, string Type, IGeometry Shores)
		{
			this.Fid = Fid;
			this.Name = Name;
			this.Type = Type;
			this.Shores = Shores;
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

		private string _type;
		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}

		private IGeometry _shores;
		public IGeometry Shores
		{
			get { return _shores; }
			set { _shores = value; }
		}

	}
}
