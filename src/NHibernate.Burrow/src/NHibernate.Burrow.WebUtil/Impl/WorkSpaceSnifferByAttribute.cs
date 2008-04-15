using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using NHibernate.Burrow.WebUtil.Attributes;

namespace NHibernate.Burrow.WebUtil.Impl
{
	public class WorkSpaceSnifferByAttribute : IWorkSpaceNameSniffer {
		public string Sniff(IHttpHandler handler) {
			WorkSpaceInfo wsi = (WorkSpaceInfo) Attribute.GetCustomAttribute(handler.GetType(), typeof(WorkSpaceInfo));
			if (wsi != null)
				return wsi.WorkSpaceName;
			return string.Empty;
		}
	}
}
