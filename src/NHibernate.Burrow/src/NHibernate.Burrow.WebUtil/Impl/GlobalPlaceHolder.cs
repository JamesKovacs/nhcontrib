using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NHibernate.Burrow.WebUtil.Exceptions;

namespace NHibernate.Burrow.WebUtil.Impl
{
    public class GlobalPlaceHolder
    {
        private const string holderId = "NHibernate.Burrow.WebUtil.GlobalPlaceHolder";
        private const string UpdatePanelId = "NHibernate.Burrow.WebUtil.GlobalPlaceHolderUpdatePanel";
        private readonly Control holder;
        private readonly Page page;

        public GlobalPlaceHolder(Page p)
        {
            this.page = p;
			holder = new PlaceHolder();
			holder.ID = holderId;
			p.PreRender += new EventHandler(p_PreRender);
        }

		void p_PreRender(object sender, EventArgs e)
		{
			if(IsInAjaxMode()) {
			    UpdatePanel up = new UpdatePanel();
				up.ID = UpdatePanelId;
				up.UpdateMode = UpdatePanelUpdateMode.Always;
				page.Form.Controls.Add(up);
				up.ContentTemplateContainer.Controls.Add(holder);
			}else {
					page.Form.Controls.Add(holder);
			}
		}

        public Control Holder
        {
            get
            {
                if (holder == null)
                {
                    throw new BurrowWebUtilException("Holder is ready after init");
                }
                return holder;
            }
        }

  

        private bool IsInAjaxMode()
        {
            ScriptManager current = ScriptManager.GetCurrent(page);
            if (current != null && current.EnablePartialRendering)
            {
                return true;
            }
            return false;
        }
    }
}