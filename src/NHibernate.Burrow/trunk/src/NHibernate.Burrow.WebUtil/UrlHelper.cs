using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using NHibernate.Burrow.NHDomain;

namespace NHibernate.Burrow.WebUtil
{
    public class UrlHelper
    {
        public  string WrapUrlWithOverSpanInfo(string originalUrl) {
           StringBuilder sb = new StringBuilder(originalUrl);

            bool firstPara = originalUrl.IndexOf("?") < 0;
            foreach (OverspanState state in DomainContext.Current.OverspanStates()) {
                if(state.Mode != OverspanMode.None 
                    && !string.IsNullOrEmpty(state.Name)
                    && !string.IsNullOrEmpty(state.Value)) {
                    sb.Append((firstPara ?"?" : "&" ));
                    sb.Append( HttpContext.Current.Server.UrlEncode( state.Name ));
                    sb.Append("=");
                    sb.Append(HttpContext.Current.Server.UrlEncode( state.Value ));
                    firstPara = false;
                }
            }
            return sb.ToString();
        }
    }
}
