using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BasicSample.Core.DataInterfaces;
using BasicSample.Data;

/// <summary>
/// Summary description for ServiceLocator
/// </summary>
public class ServiceLocator
{
    private static readonly NHibernateDaoFactory daoFactory = new NHibernateDaoFactory();

    public static IDaoFactory DaoFactory {
        get {
            return daoFactory;
        }
    }
}
