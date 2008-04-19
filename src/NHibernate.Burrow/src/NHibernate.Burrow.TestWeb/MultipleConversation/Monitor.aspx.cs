using System;
using System.Collections.Generic;
using System.Web.UI;
using NHibernate.Burrow.Test.MockEntities;

public partial class MultipleConversation_Monitor : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Util.ResetEnvironment();
            for (int i = 0; i < 4; i++)
            {
                Session["MultipleConversationStep" + i] = null;
            }
        }

        if (Session["MultipleConversationStep" + 3] != null && (int) Session["MultipleConversationStep" + 3] == 4)
        {
            Checker.CheckSpanningConversations(0);
            IList<MockEntity> result = MockEntityDAO.Instance.FindAll();
            foreach (MockEntity me in result)
            {
                Checker.AssertEqual(3, me.Number);
            }
            Checker.AssertEqual(4, result.Count);
            lStatus.Text = "Conratulatioins! Test succeeded.";
        }
    }
}