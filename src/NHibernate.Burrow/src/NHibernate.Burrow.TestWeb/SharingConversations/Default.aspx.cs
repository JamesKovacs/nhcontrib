using System;
using System.Web.UI;
using NHibernate.Burrow;

public partial class SharingConversations_Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        Facade f = new Facade();
        f.CloseDomain();
        f.BurrowEnvironment.ShutDown(); //Restart the environment to prepare a fresh start 
        f.BurrowEnvironment.Start();
        Checker.CheckSpanningConversations(0);
        f.InitializeDomain();
    }
    
}