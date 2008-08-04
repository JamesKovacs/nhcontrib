// Copyright 2007 - Ricardo Stuven (rstuven@gmail.com)
//
// This file is part of NHibernate.Spatial.
// NHibernate.Spatial is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// NHibernate.Spatial is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with NHibernate.Spatial; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using System.Reflection;
using NHibernate.Cfg;

namespace NHibernate.Spatial.Metadata
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class SpatialReferenceSystem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpatialReferenceSystem"/> class.
		/// </summary>
		public SpatialReferenceSystem()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SpatialReferenceSystem"/> class.
		/// </summary>
		/// <param name="SRID">The SRID.</param>
		/// <param name="AuthorityName">Name of the authority.</param>
		/// <param name="AuthoritySRID">The authority SRID.</param>
		/// <param name="WellKnownText">The well known text.</param>
		public SpatialReferenceSystem(int SRID, string AuthorityName, int AuthoritySRID, string WellKnownText)
		{
			this.SRID = SRID;
			this.AuthorityName = AuthorityName;
			this.AuthoritySRID = AuthoritySRID;
			this.WellKnownText = WellKnownText;
		}

		private int _SRID;
		/// <summary>
		/// Gets or sets the SRID.
		/// </summary>
		/// <value>The SRID.</value>
		public virtual int SRID
		{
			get { return _SRID; }
			set { _SRID = value; }
		}

		private string _AuthorityName;
		/// <summary>
		/// Gets or sets the name of the authority.
		/// </summary>
		/// <value>The name of the authority.</value>
		public virtual string AuthorityName
		{
			get { return _AuthorityName; }
			set { _AuthorityName = value; }
		}

		private int _AuthoritySRID;
		/// <summary>
		/// Gets or sets the authority SRID.
		/// </summary>
		/// <value>The authority SRID.</value>
		public virtual int AuthoritySRID
		{
			get { return _AuthoritySRID; }
			set { _AuthoritySRID = value; }
		}

		private string _WellKnownText;
		/// <summary>
		/// Gets or sets the well known text.
		/// </summary>
		/// <value>The well known text.</value>
		public virtual string WellKnownText
		{
			get { return _WellKnownText; }
			set { _WellKnownText = value; }
		}

	}
}
