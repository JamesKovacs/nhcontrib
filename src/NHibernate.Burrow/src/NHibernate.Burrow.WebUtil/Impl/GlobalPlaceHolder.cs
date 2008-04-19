using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NHibernate.Burrow.WebUtil.Exceptions;

namespace NHibernate.Burrow.WebUtil.Impl {
	public class GlobalPlaceHolder {
		private const string holderId = "NHibernate.Burrow.WebUtil.GlobalPlaceHolder";
		private const string UpdatePanelId = "NHibernate.Burrow.WebUtil.GlobalPlaceHolderUpdatePanel";
		private Control holder;
		private Page p;

		public GlobalPlaceHolder(Page p) {
			this.p = p;
            p.Init += new EventHandler(p_Init);
		}

		public Control Holder {
			get {
				if (holder == null)
					throw new BurrowWebUtilException("Holder is ready after init");
				return holder;
			}
		}

		private void p_Init(object sender, EventArgs e) {
			holder = IsInAjaxMode() ? GetParentInAjax(p.Form) : Normal(p.Form);
		}

		private Control Normal(HtmlForm form) {
			PlaceHolder ph = new PlaceHolder();
			ph.ID = holderId;
			form.Controls.Add(ph);
			return ph;
		}

		private Control GetParentInAjax(HtmlForm form) {
			UpdatePanel up = new UpdatePanel();
			up.ID = UpdatePanelId;
			up.UpdateMode = UpdatePanelUpdateMode.Always;
			form.Controls.Add(up);
			return up.ContentTemplateContainer;
		}

		private bool IsInAjaxMode() {
			ScriptManager current = ScriptManager.GetCurrent(p);
			if (current != null && current.EnablePartialRendering)
				return true;
			return false;
		}
	}
}