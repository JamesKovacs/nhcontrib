using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace NHibernate.Burrow.Impl
{
	internal static class LogFactory
	{
	 
		public static ILog Log {
			get {
				return log4net.LogManager.GetLogger("NHibernate.Burrow");
			}
		}
	
	}
}
