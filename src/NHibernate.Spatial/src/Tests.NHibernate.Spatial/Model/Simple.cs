using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.Model
{
	[Serializable]
	public class Simple
	{
		public Simple()
		{
		}

		public Simple(string Description, IGeometry Geometry)
		{
			this.Description = Description;
			this.Geometry = Geometry;
		}

		private long _Id;
		public virtual long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}

		private string _Description;
		public virtual string Description
		{
			get { return _Description; }
			set { _Description = value; }
		}

		private IGeometry _Geometry;
		public virtual IGeometry Geometry
		{
			get { return this._Geometry; }
			set { this._Geometry = value; }
		}
	}
}
