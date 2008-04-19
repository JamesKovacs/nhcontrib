using System;
using System.Web.UI;

namespace NHibernate.Burrow.WebUtil.Impl
{
    internal class StatefulFieldPageModule
    {
        private readonly GlobalPlaceHolder gph;
        private readonly Page page;
        protected bool dataLoaded = false;

        public StatefulFieldPageModule(Page page, GlobalPlaceHolder globalPlaceHolder)
        {
            this.page = page;
            gph = globalPlaceHolder;
            page.Init += new EventHandler(page_Init);
            page.PreRenderComplete += new EventHandler(page_PreRenderComplete);
        }

        private void page_PreRenderComplete(object sender, EventArgs e)
        {
            new StatefulFieldSaver(page, gph.Holder).Process();
        }

        private void page_Init(object sender, EventArgs e)
        {
            if (!page.IsPostBack || dataLoaded)
            {
                return;
            }
            dataLoaded = true;
            new StatefulFieldLoader(page, gph.Holder).Process();
        }
    }
}