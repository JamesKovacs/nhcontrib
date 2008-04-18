using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Burrow;
using NHibernate.Burrow.WebUtil.Impl;

namespace NHibernate.Burrow.WebUtil {
	internal class ConversationStatePageModule
	{
     
        private readonly Page page;
		private readonly GlobalPlaceHolder gph;

		public ConversationStatePageModule(Page p, GlobalPlaceHolder globalPlaceHolder)
		{
            page = p;
        	gph = globalPlaceHolder;
        	page.PreRender += new EventHandler(Page_PreRender);
        }

        private void Page_PreRender(object sender, EventArgs e) {
			Burrow.Util.WebUtil.AddConversationStates(gph.Holder);
        }
    }
}