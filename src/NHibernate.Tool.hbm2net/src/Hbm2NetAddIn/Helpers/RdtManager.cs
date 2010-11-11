using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System.IO;

namespace FelicePollano.Hbm2NetAddIn.Helpers
{
    class RdtManager:IVsRunningDocTableEvents, IRdtHelper
    {
        uint cookie;
        IVsRunningDocumentTable rdt;
        public event EventHandler<SavedProjectItemEventArgs> SavedProjectItem;
        public RdtManager(IServiceProvider provider)
        {

            rdt = provider.GetService(typeof(IVsRunningDocumentTable)) as IVsRunningDocumentTable;
            rdt.AdviseRunningDocTableEvents(this, out cookie);
            var dte = provider.GetService(typeof(DTE)) as DTE;
            
           
        }

        public ProjectItem GetProjectItemByName(string lpszname)
        {
            IVsHierarchy hierarchy;
            uint  pitemid,cookie;
            IntPtr ppunkDocData;
            if( VSConstants.S_OK == rdt.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_NoLock,lpszname,out hierarchy,out pitemid,out ppunkDocData,out cookie) )
            {
                return GetProjectItem(hierarchy, pitemid);
            }
            return null;
        }

        #region IVsRunningDocTableEvents Members

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return 0;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return 0;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return 0;
        }

        public int OnAfterSave(uint docCookie)
        {
            string path;
            IVsHierarchy hierarchy;
            uint flags, pdwReadLocks, pdwEditLocks, pitemid;
            IntPtr ppunkDocData;
            rdt.GetDocumentInfo(docCookie, out flags, out pdwReadLocks, out pdwEditLocks, out path, out hierarchy, out pitemid, out ppunkDocData);
            var prj = GetProjectItem(hierarchy,pitemid);
          
            if (null != SavedProjectItem)
            {
                SavedProjectItem(this, new SavedProjectItemEventArgs() {  ProjectItem=prj, SavedPath=path });
            }
            return 0;
        }
        public ProjectItem GetProjectItem(IVsHierarchy hierarchy,uint itemid)
        {

            object project;



            ErrorHandler.ThrowOnFailure

                (hierarchy.GetProperty(
                    itemid,
                    (int)__VSHPROPID.VSHPROPID_ExtObject,
                    out project));

            return (project as ProjectItem);

        }
        public Project GetProject(IVsHierarchy hierarchy)
        {

            object project;

            ErrorHandler.ThrowOnFailure
                (hierarchy.GetProperty(
                    (uint)VSConstants.VSITEMID.Root,
                    (int)__VSHPROPID.VSHPROPID_ExtObject,
                    out project));
            return (project as Project);

        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return 0;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return 0;
        }

        #endregion
    }
}
