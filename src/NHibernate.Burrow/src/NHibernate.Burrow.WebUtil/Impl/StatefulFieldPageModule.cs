using System;
using System.Web.UI;

namespace NHibernate.Burrow.WebUtil {
	internal class StatefulFieldPageModule
	{
        private readonly Page page;
        protected bool dataLoaded = false;

        public StatefulFieldPageModule(Page page) {
            this.page = page;
            page.PreLoad += new EventHandler(page_PreLoad);
            page.PreRenderComplete += new EventHandler(page_PreRenderComplete);
        }

        private void page_PreRenderComplete(object sender, EventArgs e) {
            new StatefulFieldSaver(page).Process();
        }

        private void page_PreLoad(object sender, EventArgs e) {
            if (!page.IsPostBack || dataLoaded)
                return;
            dataLoaded = true;
            new StatefulFieldLoader(page).Process();
        }
    }
}