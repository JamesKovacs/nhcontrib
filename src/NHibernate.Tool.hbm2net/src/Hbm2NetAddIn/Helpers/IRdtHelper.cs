using System;
using EnvDTE;
namespace FelicePollano.Hbm2NetAddIn.Helpers
{
    public interface IRdtHelper
    {
        Project GetProject(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy hierarchy);
        ProjectItem GetProjectItem(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy hierarchy, uint itemid);
        ProjectItem GetProjectItemByName(string lpszname);
    }
}
