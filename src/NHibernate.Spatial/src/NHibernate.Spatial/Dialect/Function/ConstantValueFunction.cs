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

using System.Collections;
using NHibernate.Type;
using NHibernate.Engine;
using NHibernate.Dialect.Function;
using NHibernate.SqlCommand;

namespace NHibernate.Spatial.Dialect.Function
{
	/// <summary>
	/// A function that renders a value, without needing arguments 
	/// nor empty parentheses, as if were rather a constant identifier.
	/// 
	/// This is useful to register symbols whose literal value differs 
	/// among dialects (eg. TRUE and FALSE literal values)
	/// </summary>
	public class ConstantValueFunction : ISQLFunction
	{
		private string value;
		private IType returnType;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantValueFunction"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="returnType">Type of the return.</param>
		public ConstantValueFunction(string value, IType returnType)
		{
			this.value = value;
			this.returnType = returnType;
		}

		#region ISQLFunction Members

		/// <summary>
		/// Does this function have any arguments?
		/// </summary>
		/// <value></value>
		public bool HasArguments
		{
			get { return false; }
		}

		/// <summary>
		/// If there are no arguments, are parens required?
		/// </summary>
		/// <value></value>
		public bool HasParenthesesIfNoArguments
		{
			get { return false; }
		}


		/// <summary>
		/// Render the function call as SQL.
		/// </summary>
		/// <param name="args">List of arguments</param>
		/// <param name="factory"></param>
		/// <returns>SQL fragment for the fuction.</returns>
		public SqlString Render(IList args, ISessionFactoryImplementor factory)
		{
			return new SqlString(this.value);
		}

		public IType ReturnType(IType columnType, IMapping mapping)
		{
			return this.returnType;
		}

		#endregion
	}
}
