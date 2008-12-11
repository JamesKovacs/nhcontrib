using System;

namespace NHibernate.Burrow.WebUtil.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class WorkSpaceInfo : Attribute
    {
        private readonly string workSpaceName;

        public WorkSpaceInfo(string workSpaceName)
        {
            this.workSpaceName = workSpaceName;
        }

        public string WorkSpaceName
        {
            get { return workSpaceName; }
        }
    }
}