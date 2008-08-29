using System;
using NHibernate.Engine;

namespace NHibernate.Annotations
{
	public class ExecuteUpdateResultCheckStyleConverter
	{
		public static ExecuteUpdateResultCheckStyle Convert(ResultCheckStyle @enum)
		{
			switch (@enum)
			{
				case ResultCheckStyle.Count:
					return ExecuteUpdateResultCheckStyle.Count;

				case ResultCheckStyle.None:
					return ExecuteUpdateResultCheckStyle.None;

				case ResultCheckStyle.Param:
					//TODO: implement Param
					//return ExecuteUpdateResultCheckStyle.None;
					throw new NotImplementedException("ExecuteUpdateResultCheckStyle.Param is not supported.");

				default:
					return default(ExecuteUpdateResultCheckStyle);
			}
		}
	}
}