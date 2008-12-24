using System.Collections.Generic;
using System.Persistence;
using System.Reflection;
using NHibernate.Mapping;

namespace NHibernate.Cfg
{
	public class AbstractPropertyHolder : IPropertyHolder
	{
		protected IPropertyHolder parent;
		private Dictionary<string, ColumnAttribute[]> holderColumnOverride;
        private Dictionary<string, ColumnAttribute[]> currentPropertyColumnOverride;
		private Dictionary<string, JoinColumnAttribute[]> holderJoinColumnOverride;
		private Dictionary<string, JoinColumnAttribute[]> currentPropertyJoinColumnOverride;
		private string path;
		private ExtendedMappings mappings;

		public string ClassName
		{
			get { throw new System.NotImplementedException(); }
		}

		public string EntityOwnerClassName
		{
			get { throw new System.NotImplementedException(); }
		}

		public Table Table
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsComponent
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsEntity
		{
			get { throw new System.NotImplementedException(); }
		}

		public string Path
		{
			get { return path; }
		}

		public string EntityName
		{
			get { throw new System.NotImplementedException(); }
		}

		public IKeyValue Identifier
		{
			get { throw new System.NotImplementedException(); }
		}

		public void AddProperty(Property prop)
		{
			throw new System.NotImplementedException();
		}

		public PersistentClass GetPersistentClass()
		{
			throw new System.NotImplementedException();
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

	    private Dictionary<string, JoinColumnAttribute[]> BuildJoinColumnOverride(PropertyInfo property, string path)
		{
			throw new System.NotImplementedException();
		}

		public virtual void SetParentProperty(string parentProperty)
		{
			throw new AssertionFailure("Setting the parent property to a non component");
		}

		public ColumnAttribute[] GetOverriddenColumn(string propertyName)
		{
			throw new System.NotImplementedException();
		}

		public JoinColumnAttribute[] GetOverriddenJoinColumn(string propertyName)
		{
			throw new System.NotImplementedException();
		}

		public void AddProperty(Property prop, Ejb3Column[] columns)
		{
			throw new System.NotImplementedException();
		}

		public Join AddJoin(JoinTableAttribute joinTableAnn, bool noDelayInPkColumnCreation)
		{
			throw new System.NotImplementedException();
		}
	}
}