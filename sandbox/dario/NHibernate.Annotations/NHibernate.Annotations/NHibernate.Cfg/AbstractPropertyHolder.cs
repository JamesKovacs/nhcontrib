using System.Collections.Generic;
using System.Persistence;
using System.Reflection;
using NHibernate.Cfg;

namespace NHibernate.Cfg
{
	public class AbstractPropertyHolder
	{
		protected PropertyHolder parent;
		private Dictionary<string, ColumnAttribute[]> holderColumnOverride;
        private Dictionary<string, ColumnAttribute[]> currentPropertyColumnOverride;
		private Dictionary<string, JoinColumn[]> holderJoinColumnOverride;
		private Dictionary<string, JoinColumn[]> currentPropertyJoinColumnOverride;
		private string path;
		private ExtendedMappings mappings;
        
		public string Path
		{
			get { return path; }
		}

		protected void SetCurrentProperty(PropertyInfo property)
		{
			if (property == null)
			{
				this.currentPropertyColumnOverride = null;
				this.currentPropertyJoinColumnOverride = null;
			}
			else
			{
				this.currentPropertyColumnOverride = BuildColumnOverride(property,Path);
				if (this.currentPropertyColumnOverride.Count == 0)
				{
					this.currentPropertyColumnOverride = null;
				}
				this.currentPropertyJoinColumnOverride = BuildJoinColumnOverride(property,Path);
				if (this.currentPropertyJoinColumnOverride.Count == 0)
				{
					this.currentPropertyJoinColumnOverride = null;
				}
			}
		}

        private void BuildHierarchyColumnOverride(System.Type element)
        {
            throw new System.NotImplementedException();
        }

	    private Dictionary<string, ColumnAttribute[]> BuildColumnOverride(PropertyInfo property, string path)
		{
			throw new System.NotImplementedException();
		}

	    private Dictionary<string, JoinColumn[]> BuildJoinColumnOverride(PropertyInfo property, string path)
		{
			throw new System.NotImplementedException();
		}

		public virtual void SetParentProperty(string parentProperty)
		{
			throw new AssertionFailure("Setting the parent property to a non component");
		}

	}
}