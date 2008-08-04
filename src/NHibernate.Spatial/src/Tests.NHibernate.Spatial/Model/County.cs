using System;
using GeoAPI.Geometries;

namespace Tests.NHibernate.Spatial.Model
{
	[Serializable]
	public class County
	{
		public County()
		{
		}

		public County(string Name, string State, IGeometry Boundaries)
		{
			this.Name = Name;
			this.State = State;
			this.Boundaries = Boundaries;
		}

		private long _Id;
		public virtual long Id
		{
			get { return _Id; }
			set { _Id = value; }
		}

		private string _Name;
		public virtual string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}


		private string _State;
		public virtual string State
		{
			get { return _State; }
			set { _State = value; }
		}

		private IGeometry _Boundaries;
		public virtual IGeometry Boundaries
		{
			get { return this._Boundaries; }
			set { this._Boundaries = value; }
		}
	}
}
