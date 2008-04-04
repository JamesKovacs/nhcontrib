using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Burrow;

namespace NHibernate.Burrow.WebUtil {
    public class ConversationStatePageModule {
        private const string PlaceHolderName = "ConversationalStatePlaceHolder";
        private const string UpdatePanelId = "ConversationalStateUpdatePanel";
        private Control controlForHoldState;
        private Page page;

        public ConversationStatePageModule(Page p) {
            page = p;
            page.PreRender += new EventHandler(Page_PreRender);
            page.Init += new EventHandler(Page_Init);
        }

        private void Page_PreRender(object sender, EventArgs e) {
            AddConversationStates(controlForHoldState);
        }

        private void Page_Init(object sender, EventArgs e) {
            if (IsInAjaxMode())
                controlForHoldState = PrepareAjaxControl(page.Form);
            else
                controlForHoldState = page.Form;
        }

        private bool IsInAjaxMode() {
            ScriptManager current = ScriptManager.GetCurrent(page);
            if(current != null && current.EnablePartialRendering)
                return true;
            return false;
        }

        private Control PrepareAjaxControl(Control parentControl) {
            UpdatePanel up = new UpdatePanel();
            up.ID = UpdatePanelId;
            up.UpdateMode = UpdatePanelUpdateMode.Always;
            PlaceHolder ph = new PlaceHolder();
            ph.ID = PlaceHolderName;
            up.ContentTemplateContainer.Controls.Add(ph);
            parentControl.Controls.Add(up);
            return ph;
        }

        private void AddConversationStates(Control c) {
            foreach (SpanState os in SpanState.CurrentStates()){
                os.CleanCookies(HttpContext.Current); 
                os.AddOverspanState(c);
            }
        }
    }
}