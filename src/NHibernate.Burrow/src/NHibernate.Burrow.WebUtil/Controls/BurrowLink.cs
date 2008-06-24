using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace NHibernate.Burrow.WebUtil.Controls
{
    public class BurrowLink : HyperLink
    {
        protected override void OnPreRender(EventArgs e)
        {
            NavigateUrl = new Burrow.Util.WebUtil().WrapUrlWithConversationInfo(NavigateUrl);
            base.OnPreRender(e);
        }
    }
}
