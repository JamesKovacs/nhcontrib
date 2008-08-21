using System;
using System.Collections.Generic;
using System.Persistence;
using NHibernate.Annotations.Cfg.Annotations;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace NHibernate.Annotations.NHibernate.Cfg
{
    public class ClassPropertyHolder : AbstractPropertyHolder
    {
        private EntityBinder entityBinder;
        private IDictionary<string, Join> joins;
        [NonSerialized] private IDictionary<String, Join> joinsPerRealTableName;
        private PersistentClass persistentClass;

        public String EntityName
        {
            get { return persistentClass.EntityName; }
        }

        public String ClassName
        {
            get { return persistentClass.ClassName; }
        }

        public String EntityOwnerClassName
        {
            get { return ClassName; }
        }

        public Table Table
        {
            get { return persistentClass.Table; }
        }

        public bool IsComponent
        {
            get { return false; }
        }

        public bool IsEntity
        {
            get { return true; }
        }

        public PersistentClass PersistentClass
        {
            get { return persistentClass; }
        }

        public IKeyValue Identifier
        {
            get { return persistentClass.Identifier; }
        }

        public void AddProperty(Property prop, Ejb3Column[] columns)
        {
            //Ejb3Column.checkPropertyConsistency( ); //already called earlier
            if (columns[0].IsSecondary)
            {
                //TODO move the getJoin() code here?
                columns[0].Join.AddProperty(prop);
            }
            else
            {
                AddProperty(prop);
            }
        }

        public void AddProperty(Property prop)
        {
            if (prop.Value is Component)
            {
                //TODO handle quote and non quote table comparison
                String tableName = prop.Value.Table.Name;
                if (GetJoinsPerRealTableName().ContainsKey(tableName))
                {
                    GetJoinsPerRealTableName()[tableName].AddProperty(prop); //TODO review the Get from dictionary.
                }
                else
                {
                    persistentClass.AddProperty(prop);
                }
            }
            else
            {
                persistentClass.AddProperty(prop);
            }
        }

        public Join AddJoin(JoinTable joinTableAnn, bool noDelayInPkColumnCreation)
        {
            Join join = entityBinder.AddJoin(joinTableAnn, this, noDelayInPkColumnCreation);
            joins = entityBinder.SecondaryTables;
            return join;
        }

        /// <summary>
        /// Needed for proper compliance with naming strategy, the property table
        /// can be overriden if the properties are part of secondary tables
        /// </summary>
        private IDictionary<String, Join> GetJoinsPerRealTableName()
        {
            if (joinsPerRealTableName == null)
            {
                joinsPerRealTableName = new Dictionary<String, Join>(joins.Count);
                foreach (Join join in joins.Values)
                {
                    joinsPerRealTableName.Add(join.Table.Name, join);
                }
            }
            return joinsPerRealTableName;
        }
    }
}