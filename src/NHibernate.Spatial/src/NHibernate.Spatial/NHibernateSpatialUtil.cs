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

#if NH_HQL_FUNCTION_MAPPING
using System;
#else
using System.Collections;
using System.Collections.Generic;
using NHibernate.Dialect.Function;
using NHibernate.Spatial.Dialect.Function;
using NHibernate.Type;
#endif

namespace NHibernate.Spatial
{
	/// <summary>
	/// 
	/// </summary>
	public static class NHibernateSpatialUtil
	{
		/// <summary>
		/// If you need to use HQL functions compatible among different dialects
		/// (those starting with "NHSP.") and you're using NHibernate 1.2.0, then
		/// you must call this function before calling CreateQuery.
		/// PrepareQuery provides a very limited workaround for the lack of
		/// HQL function mapping in NHibernate 1.2.0, so use it carefully.
		/// 
		/// However, if you are using a NHibernate assembly compiled from the
		/// SVN trunk since revision 2709, you won't need this.
		/// (See http://jira.nhibernate.org/browse/NH-628)
		/// </summary>
		/// <param name="dialect"></param>
		/// <param name="queryString"></param>
		/// <returns></returns>
#if NH_HQL_FUNCTION_MAPPING
		[Obsolete]
#endif
		public static string PrepareQuery(NHibernate.Dialect.Dialect dialect, string queryString)
		{
#if !NH_HQL_FUNCTION_MAPPING
			Dictionary<string, IType> pendingRegistration = new Dictionary<string, IType>();
			foreach (KeyValuePair<string, ISQLFunction> entry in dialect.Functions)
			{
				string functionName = (string)entry.Key;
				ISQLFunction function = (ISQLFunction)entry.Value;
				if (function is SpatialStandardFunction)
				{
					int argsCount = 100; // spare arguments
					SpatialStandardSafeFunction safeFunction = function as SpatialStandardSafeFunction;
					if (safeFunction != null)
					{
						argsCount = safeFunction.AllowedArgsCount;
					}
					object[] args = new object[argsCount];
					string rendered = function.Render(args, null);
						string dialectFunctionName = rendered.Substring(0, rendered.IndexOf("("));
						if (!dialect.Functions.ContainsKey(dialectFunctionName) &&
							!string.IsNullOrEmpty(dialectFunctionName))
						{
							// We need to register it or we'll get an exception:
							// "undefined alias or unknown mapping"
							pendingRegistration.Add(dialectFunctionName, function.ReturnType(null, null));
						}
						queryString = queryString.Replace(functionName, dialectFunctionName);
					}
				else if (function is ConstantValueFunction)
				{
					string rendered = function.Render(null, null);
					queryString = queryString.Replace(functionName, rendered);
				}
			}
			foreach(KeyValuePair<string,IType> item in pendingRegistration)
			{
				dialect.Functions.Add(item.Key, new SpatialStandardFunction(item.Key, item.Value));
			}
#endif
			return queryString;
		}

		/// <summary>
		/// If you need to use HQL functions compatible among different dialects
		/// (those starting with "NHSP.") and you're using NHibernate 1.2.0, then
		/// you must call this function before calling CreateQuery.
		/// PrepareQuery provides a very limited workaround for the lack of
		/// HQL function mapping in NHibernate 1.2.0, so use it carefully.
		/// 
		/// However, if you are using a NHibernate assembly compiled from the
		/// SVN trunk since revision 2709, you won't need this.
		/// (See http://jira.nhibernate.org/browse/NH-628)
		/// </summary>
		/// <param name="session"></param>
		/// <param name="queryString"></param>
		/// <returns></returns>
#if NH_HQL_FUNCTION_MAPPING
		[Obsolete]
#endif
		public static string PrepareQuery(ISession session, string queryString)
		{
			return PrepareQuery((session.SessionFactory as NHibernate.Engine.ISessionFactoryImplementor).Dialect, queryString);
		}

		/// <summary>
		/// Utility function to create queries "prepared" for NHibernate.Spatial.
		/// See <see cref="NHibernateSpatialUtil.PrepareQuery"/>.
		/// </summary>
		/// <param name="session"></param>
		/// <param name="queryString"></param>
		/// <returns></returns>
#if NH_HQL_FUNCTION_MAPPING
		[Obsolete]
#endif
		public static IQuery CreateQuery(ISession session, string queryString)
		{
			return session.CreateQuery(PrepareQuery(session, queryString));
		}
	}
}
