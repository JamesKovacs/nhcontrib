using System;
using System.Web;
using NHibernate.Burrow.WebUtil.Attributes;

namespace NHibernate.Burrow.WebUtil.Impl
{
    public class WorkSpaceSnifferByAttribute : IWorkSpaceNameSniffer
    {
        #region IWorkSpaceNameSniffer Members

        public string Sniff(IHttpHandler handler)
        {
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