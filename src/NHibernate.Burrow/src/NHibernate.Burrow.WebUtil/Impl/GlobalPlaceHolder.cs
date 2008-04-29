using System;
using System.Collections.Generic;
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
        private UpdatePanel up;
		
        public GlobalPlaceHolder(Page p)
        {
            this.page = p;
			holder = new PlaceHolder();
			holder.ID = holderId;
            p.Init += new EventHandler(p_Init);
			p.PreRender += new EventHandler(p_PreRender);
        }

        void p_Init(object sender, EventArgs e)
        {
           if(IsInAjaxMode())
           {
               up = new UpdatePanel();
               up.ID = UpdatePanelId;
               up.UpdateMode = UpdatePanelUpdateMode.Always;
               page.Form.Controls.Add(up);
           }
        }

		void p_PreRender(object sender, EventArgs e)
		{
            if (IsInAjaxMode())
            {
				up.ContentTemplateContainer.Controls.Add(holder);
			}else {
					page.Form.Controls.Add(holder);
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

        public void AddPostBackFields(IDictionary<String,string> fields)
        {
            foreach (KeyValuePair<string, string> pair in fields)
            {
                AddPostBackField(pair.Key,pair.Value);
            }
        }

        public void AddPostBackField(string key, string val)
        {
            if(!string.IsNullOrEmpty(val))
            {
                Literal l = new Literal(); //use literal instead of HiddenField to precisely control Id and name
                l.Text = string.Format("<input type='hidden' Name='{0}' ID='{0}' value='{1}' />", key, val); 
                holder.Controls.Add(l);
            }
        }
    }
}