using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NHibernate.Burrow;

/// <summary>
/// Summary description for Checker
/// </summary>
public class Checker
{
    static Facade f = new Facade();
   

    public static void CheckSpanningConversations(int numOfSpanning)
    {
        int conversations = f.BurrowEnvironment.SpanningConversations;
        if(  conversations != numOfSpanning )
           throw new AssertException("Expected spanning conversation "  + numOfSpanning + " but is " + conversations );
    }

    public static void AssertEqual(object expected , object val)
    {
        if (expected == null && val == null)
            return;
        if (expected != null && expected.Equals(val))
           return;
          
        throw new AssertException("Expected " + expected + " but was " + val);

    }
}
