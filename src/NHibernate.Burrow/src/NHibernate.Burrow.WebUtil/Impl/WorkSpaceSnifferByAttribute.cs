using System;
using System.Web;
using NHibernate.Burrow.WebUtil.Attributes;

namespace NHibernate.Burrow.WebUtil.Impl
{
    internal class WorkSpaceSnifferByAttribute : IWorkSpaceNameSniffer
    {
        #region IWorkSpaceNameSniffer Members

        public string Sniff(IHttpHandler handler)
        {
        	if (handler == null) 
        	{
        		return string.Empty;
        	}
            WorkSpaceInfo wsi = (WorkSpaceInfo) Attribute.GetCustomAttribute(handler.GetType(), typeof (WorkSpaceInfo));
            if (wsi != null)
            {
                return wsi.WorkSpaceName;
            }
            return string.Empty;
        }

        #endregion
    }
}
