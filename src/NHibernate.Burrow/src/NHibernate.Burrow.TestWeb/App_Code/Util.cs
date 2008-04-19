using NHibernate.Burrow;
using NHibernate.Burrow.Util;

/// <summary>
/// Summary description for Util
/// </summary>
public class Util
{
    public static void ResetEnvironment()
    {
        SchemaUtil su = new SchemaUtil();
        su.CreateSchemas();
        BurrowFramework f = new BurrowFramework();
        f.CloseWorkSpace();
        f.BurrowEnvironment.ShutDown(); //Restart the environment to prepare a fresh start 
        f.BurrowEnvironment.Start();
        Checker.CheckSpanningConversations(0);
        f.InitWorkSpace();
    }
}