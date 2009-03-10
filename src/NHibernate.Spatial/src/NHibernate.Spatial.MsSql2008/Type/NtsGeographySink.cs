// Copyright 2008 - Ricardo Stuven (rstuven@gmail.com)
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
using System.Collections.Generic;
using GeoAPI.Geometries;
using GisSharpBlog.NetTopologySuite.Geometries;
using Microsoft.SqlServer.Types;

namespace NHibernate.Spatial.Type
{
	internal class NtsGeographySink : IGeographySink
	{
		private IGeometry _geometry;
		private int _srid;
		private Stack<OpenGisGeographyType> _types = new Stack<OpenGisGeographyType>();
		private List<ICoordinate> _coordinates = new List<ICoordinate>();
		private List<ICoordinate[]> _rings = new List<ICoordinate[]>();
		private List<IGeometry> _geometries = new List<IGeometry>();
		private bool _inFigure = false;

		public IGeometry ConstructedGeometry
		{
			get { return _geometry; }
		}

		private void AddCoordinate(double x, double y, double? z, double? m)
		{
			ICoordinate coordinate;
			if (z.HasValue)
			{
				coordinate = new Coordinate(x, y, z.Value);
			}
			else
			{
				coordinate = new Coordinate(x, y);
			}
			_coordinates.Add(coordinate);
		}

		#region IGeometrySink Members

		public void AddLine(double x, double y, double? z, double? m)
		{
			if (!_inFigure)
			{
				throw new ApplicationException();
			}
			AddCoordinate(x, y, z, m);
		}

		public void BeginFigure(double x, double y, double? z, double? m)
		{
			if (_inFigure)
			{
				throw new ApplicationException();
			}
			_coordinates = new List<ICoordinate>();
			AddCoordinate(x, y, z, m);
			_inFigure = true;
		}

		public void BeginGeography(OpenGisGeographyType type)
		{
			_types.Push(type);
		}

		public void EndFigure()
		{
			OpenGisGeographyType type = _types.Peek();
			if (type == OpenGisGeographyType.Polygon)
			{
				_rings.Add(_coordinates.ToArray());
			}
			_inFigure = false;
		}

		public void EndGeography()
		{
			IGeometry geometry = null;

			OpenGisGeographyType type = _types.Pop();

			switch (type)
			{
				case OpenGisGeographyType.Point:
					geometry = BuildPoint();
					break;
				case OpenGisGeographyType.LineString:
					geometry = BuildLineString();
					break;
				case OpenGisGeographyType.Polygon:
					geometry = BuildPolygon();
					break;
				case OpenGisGeographyType.MultiPoint:
					geometry = BuildMultiPoint();
					break;
				case OpenGisGeographyType.MultiLineString:
					geometry = BuildMultiLineString();
					break;
				case OpenGisGeographyType.MultiPolygon:
					geometry = BuildMultiPolygon();
					break;
				case OpenGisGeographyType.GeometryCollection:
					geometry = BuildGeometryCollection();
					break;
			}

			if (_types.Count == 0)
			{
				_geometry = geometry;
				_geometry.SRID = _srid;
			}
			else
			{
				_geometries.Add(geometry);
			}
		}

		private IGeometry BuildPoint()
		{
			return new Point(_coordinates[0]);
		}

		private LineString BuildLineString()
		{
			return new LineString(_coordinates.ToArray());
		}

		private IGeometry BuildPolygon()
		{
			if (_rings.Count == 0)
			{
				return Polygon.Empty;
			}
			else
			{
				ILinearRing shell = new LinearRing(_rings[0]);
				ILinearRing[] holes =
					_rings.GetRange(1, _rings.Count - 1)
						.ConvertAll<ILinearRing>(delegate(ICoordinate[] coordinates)
						{
							return new LinearRing(coordinates);
						}).ToArray();
				_rings.Clear();
				return new Polygon(shell, holes);
			}
		}

		private IGeometry BuildMultiPoint()
		{
			IPoint[] points =
				_geometries.ConvertAll<IPoint>(delegate(IGeometry g)
				{
					return g as IPoint;
				}).ToArray();
			return new MultiPoint(points);
		}

		private IGeometry BuildMultiLineString()
		{
			ILineString[] lineStrings =
				_geometries.ConvertAll<ILineString>(delegate(IGeometry g)
				{
					return g as ILineString;
				}).ToArray();
			return new MultiLineString(lineStrings);
		}

		private IGeometry BuildMultiPolygon()
		{
			IPolygon[] polygons =
				_geometries.ConvertAll<IPolygon>(delegate(IGeometry g)
				{
					return g as IPolygon;
				}).ToArray();
			return new MultiPolygon(polygons);
		}

		private GeometryCollection BuildGeometryCollection()
		{
			return new GeometryCollection(_geometries.ToArray());
		}

		public void SetSrid(int srid)
		{
			_srid = srid;
		}

		#endregion
	}
}
