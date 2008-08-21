using System.Collections.Generic;
using System.Persistence;
using System.Reflection;
using NHibernate.Annotations.NHibernate.Cfg;
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

		protected void setCurrentProperty(PropertyInfo property)
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

        protected virtual Dictionary<string, ColumnAttribute[]> BuildColumnOverride(PropertyInfo property, string path)
		{
			throw new System.NotImplementedException();
		}

		protected virtual Dictionary<string, JoinColumn[]> BuildJoinColumnOverride(PropertyInfo property, string path)
		{
			throw new System.NotImplementedException();
		}

		public virtual void setParentProperty(string parentProperty)
		{
			throw new AssertionFailure("Setting the parent property to a non component");
		}

	}
}