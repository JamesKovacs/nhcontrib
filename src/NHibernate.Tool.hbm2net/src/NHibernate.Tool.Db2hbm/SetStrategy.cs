using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect.Schema;

namespace NHibernate.Tool.Db2hbm
{
    public class SetStrategy:KeyAwareMetadataStrategy
    {
        GenerationContext context;
        protected override void OnProcess(GenerationContext context)
        {
            this.context = context;
            WireSets();
        }

        private void WireSets()
        {
            foreach (string tableToWire in FkForTables.Keys)
            {
                Dictionary<string,int> multicolls = new Dictionary<string,int>();
                var kinfo = FkForTables[tableToWire];
                foreach (var key in kinfo.Keys)
                {
                    var kdef = kinfo[key];
                    ITableMetadata ptable = GetPrimaryTable(kdef);
                    ITableMetadata ctable = GetForeignTable(kdef);

                    if (null != ptable)
                    {
                        var collectingClass = context.Model.GetClassFromTableName(ptable.Name);
                        var collectedClass = context.Model.GetClassFromTableName(ctable.Name);
                        if (!multicolls.ContainsKey(collectedClass.name))
                            multicolls[collectedClass.name] = 0;
                        set s = new set();
                        context.Model.AddCollectionToEntity(collectingClass.name, s);
                        s.table = collectedClass.table;
                        s.name = context.NamingStrategy.GetNameForCollection(collectedClass.name, multicolls[collectedClass.name]);
                        s.key = GenerateCollectionKey(kdef);

                        var pkctable = GetKeyColumns(ctable);
                        if (pkctable.Length > 0) // collected object is an entity
                        {
                            s.Item = new onetomany() { @class = context.Model.GetClassFromTableName(ctable.Name).name };
                        }
                        else
                        {
                            var notanentity = context.Model.GetClassFromTableName(ctable.Name);
                            property[] props = context.Model.GetPropertyOfEntity(notanentity.name);
                            if (props.Length == 1)
                            {
                                //use an element
                                element e = new element();
                                e.column = props[0].column ?? props[0].name;
                                e.type1 = props[0].type1;
                                e.type = props[0].type;
                                e.precision = props[0].precision;
                                e.length = props[0].length;
                                s.Item = e;
                            }
                            else
                            {
                                //use a composite element
                                compositeelement ce = new compositeelement();
                                ce.@class = context.NamingStrategy.GetClassNameForCollectionComponent(ctable.Name);
                                ce.Items = context.Model.GetPropertyOfEntity(notanentity.name);
                                s.Item = ce;
                            }
                            context.Model.RemoveEntity(notanentity.name);
                        }
                    }
                }
            }
        }

        private key GenerateCollectionKey(IForeignKeyColumnInfo[] kdef)
        {
            key key;
            key = new key();
            if (kdef.Length == 1)
            {
                key.column1 = kdef[0].ForeignKeyColumnName;
            }
            else
            {
                key.column = kdef.Select(q => new column() { name = q.ForeignKeyColumnName }).ToArray();
            }
            return key;
        }

        private ITableMetadata GetForeignTable(IForeignKeyColumnInfo[] iForeignKeyColumnInfo)
        {
            //we should have only one table as foreign
            ITableMetadata pname = null;
            foreach (var s in iForeignKeyColumnInfo)
            {
                if (pname != null && s.ForeignKeyTableName != pname.Name)
                {
                    logger.Warn(string.Format("Inconsistent key definition. Set not created"));
                    return null;
                }
                pname = new TableMetadata(s.ForeignKeyTableCatalog,s.ForeignKeyTableSchema,s.ForeignKeyTableName);
            }
            return pname;
        }

        private ITableMetadata GetPrimaryTable(IForeignKeyColumnInfo[] iForeignKeyColumnInfo)
        {
            //we should have only one table as primary
            ITableMetadata pname = null;
            foreach (var s in iForeignKeyColumnInfo)
            {
                if (pname != null && s.PrimaryKeyTableName != pname.Name)
                {
                    logger.Warn(string.Format("Inconsistent key definition. Set not created"));
                    return null;
                }
                pname = new TableMetadata(s.PrimaryKeyTableCatalog,s.PrimaryKeyTableSchema,s.PrimaryKeyTableName);
            }
            return pname;
        }

        class TableMetadata : ITableMetadata
        {
            public TableMetadata(string catalog,string schema,string name)
            {
                Catalog = catalog;
                Schema = schema;
                Name = name;
            }
            #region ITableMetadata Members

            public string Catalog
            {
                get;
                private set;
            }

            public IColumnMetadata GetColumnMetadata(string columnName)
            {
                throw new NotImplementedException();
            }

            public IForeignKeyMetadata GetForeignKeyMetadata(string keyName)
            {
                throw new NotImplementedException();
            }

            public IIndexMetadata GetIndexMetadata(string indexName)
            {
                throw new NotImplementedException();
            }

            public string Name
            {
                get;
                private set;
            }

            public bool NeedPhysicalConstraintCreation(string fkName)
            {
                throw new NotImplementedException();
            }

            public string Schema
            {
                get;
                private set;
            }

            #endregion
        }

        
    }
}
