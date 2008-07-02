using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NHibernate.Burrow;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.WebUtil.Attributes;

public partial class ConversationStates_ConversationLazyLoad : System.Web.UI.Page
{
    [ConversationalField] protected MockEntity me;

    BurrowFramework bf = new BurrowFramework();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            MockEntity m = new MockEntity();
            m.Save();
            m.StringList.Add("testString");
            m.StringList.Add("testString2");
            
            bf.GetSession().Clear();
            me = MockEntityDAO.Instance.Get(m.Id); //me is now holding an uninitilized mockEntity
            bf.CurrentConversation.SpanWithPostBacks();
        }
    }

    protected void Next(object sender, EventArgs e)
    {
        foreach (string s in me.StringList) //lazyload without getting the sessionfirst
        {
            Checker.AssertEqual( s.Contains("testString"), true  ); 
        }
        SuccessMessage1.Show();
        me.Delete();
        btnNext.Visible = false;
        bf.CurrentConversation.FinishSpan();
    }
}
