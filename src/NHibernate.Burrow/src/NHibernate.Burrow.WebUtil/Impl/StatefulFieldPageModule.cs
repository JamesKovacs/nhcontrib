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
            page.PreLoad += new EventHandler(LoadData);
            page.PreRenderComplete += new EventHandler(page_PreRenderComplete);
        }

        private void page_PreRenderComplete(object sender, EventArgs e)
        {
            new StatefulFieldSaver(page, gph).Process();
        }


		/// <summary>
		/// process has to happen after all controls are initiated otherwise will cause ASP.net control initiation problem - sub control even failed to register
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LoadData(object sender, EventArgs e)
        {
            if (!page.IsPostBack || dataLoaded)
            {
                return;
            }
            dataLoaded = true;
            new StatefulFieldLoader(page, gph).Process();
        }
    }
}