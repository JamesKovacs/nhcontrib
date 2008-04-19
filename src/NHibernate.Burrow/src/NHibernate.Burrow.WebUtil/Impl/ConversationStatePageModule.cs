using System;
using System.Web.UI;

namespace NHibernate.Burrow.WebUtil.Impl
{
    internal class ConversationStatePageModule
    {
        private readonly GlobalPlaceHolder gph;
        private readonly Page page;

        public ConversationStatePageModule(Page p, GlobalPlaceHolder globalPlaceHolder)
        {
            page = p;
            gph = globalPlaceHolder;
            page.PreRender += new EventHandler(Page_PreRender);
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            Util.WebUtil.AddConversationStates(gph.Holder);
        }
    }
}