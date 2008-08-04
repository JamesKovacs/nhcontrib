using System;
using Castle.ActiveRecord;
using GeoAPI.Geometries;

namespace NHibernate_Spatial_Tests.Model
{
	[ActiveRecord]
	public class County : TestModelBase<County>
	{

		private string _Name;

		[Property(ColumnType = "String")]
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}


		private string _State;

		[Property(ColumnType = "String")]
		public string State
		{
			get { return _State; }
			set { _State = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		private IGeometry _Boundaries;
		[Property("the_geom", ColumnType = "NHibernate.Spatial.Type.GeometryType,NHibernate.Spatial")]
		public virtual IGeometry Boundaries
		{
			get { return this._Boundaries; }
			set { this._Boundaries = value; }
		}

	}
}
