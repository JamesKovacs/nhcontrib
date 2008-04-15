using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace NHibernate.Burrow.WebUtil
{

	/// <summary>
	/// a helper class that tells what's the workspace name of a <see cref="IHttpHandler"/>
	/// </summary>
	/// <remarks>
	/// implement this and set IBurrowConfig.WorkSpaceNameSniffer to the type name of your implementation to realize your own workspace management.
	/// By default, system will use attribute placed on handler the get the current workspacename
	/// </remarks>
	public interface IWorkSpaceNameSniffer
	{
		string Sniff(IHttpHandler handler);
	}
}
