using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;

namespace FelicePollano.Hbm2NetAddIn.Helpers
{
    class ProjectEventManager:IVsSolutionEvents
    {
        class ProjectEventSink : IVsHierarchyEvents
        {
            IVsHierarchy hierarchy;
            public ProjectEventSink(IVsHierarchy hierarchy)
            {
                this.hierarchy = hierarchy;
            }
            #region IVsHierarchyEvents Members

            public int OnInvalidateIcon(IntPtr hicon)
            {
                return 0;
            }

            public int OnInvalidateItems(uint itemidParent)
            {
                return 0;
            }

            public int OnItemAdded(uint itemidParent, uint itemidSiblingPrev, uint itemidAdded)
            {
                object res;
                hierarchy.GetProperty(itemidAdded,(int) __VSHPROPID.VSHPROPID_SaveName, out res);
                return 0;
            }

            public int OnItemDeleted(uint itemid)
            {
                return 0;
            }

            public int OnItemsAppended(uint itemidParent)
            {
                return 0;
            }

            public int OnPropertyChanged(uint itemid, int propid, uint flags)
            {
                if (propid == (int)__VSHPROPID.VSHPROPID_SaveName)
                {
 
                }
                return 0;
            }

            #endregion
        }
        IDictionary<IVsHierarchy, uint> projectCookies;
        public ProjectEventManager(IServiceProvider provider)
        {
            uint solutionEventCookie;
            projectCookies = new Dictionary<IVsHierarchy, uint>();
            var sol = provider.GetService(typeof(IVsSolution)) as IVsSolution;
            sol.AdviseSolutionEvents(this, out solutionEventCookie);
        }

        #region IVsSolutionEvents Members

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            return 0;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
           
            return 0;
        }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            uint cookie;
            pHierarchy.AdviseHierarchyEvents(new ProjectEventSink(pHierarchy), out cookie);
            projectCookies[pHierarchy] = cookie;
            return 0;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            return 0;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            
            if (projectCookies.ContainsKey(pHierarchy))
            {
                pHierarchy.UnadviseHierarchyEvents(projectCookies[pHierarchy]);
            }
            return 0;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            return 0;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return 0;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            
            return 0;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return 0;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return 0;
        }
        #endregion

       
    }
}
