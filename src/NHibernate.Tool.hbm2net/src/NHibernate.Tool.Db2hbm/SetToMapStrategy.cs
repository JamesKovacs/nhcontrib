using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public class SetToMapStrategy:KeyAwareMetadataStrategy
    {
        GenerationContext context;
        public TypeConverter typeConverter { set; protected get; }
        List<set> setToRemove;
        List<string> entitiesToRemove = new List<string>();
        protected override void OnProcess(GenerationContext context)
        {
            this.context = context;
            foreach (var entity in context.Model.GetEntities())
            {
                logger.Debug(string.Format("{0} working on:", GetType().Name) + entity.table);
                setToRemove = new List<set>();
                var sets = context.Model.GetCollectionsOfEntity(entity.name).OfType<set>();
                foreach( var set in sets )
                    CheckIfCanBeMap(set,entity);
                foreach (var rem in setToRemove)
                    context.Model.RemoveCollectionFromEntity(entity.name, rem);
            }
            foreach (var erem in entitiesToRemove)
            {
                context.Model.RemoveEntity(erem);
            }
        }

        private void CheckIfCanBeMap(set set, @class entity)
        {
            if (set.Item is onetomany)
            {
                var collectedEntity = context.Model.GetClassFromEntityName((set.Item as onetomany).@class);
                if (collectedEntity.Item is compositeid)
                {
                    var cid = collectedEntity.Item as compositeid;
                    List<string> setColumn = new List<string>();
                    if (!string.IsNullOrEmpty(set.key.column1))
                        setColumn.Add(set.key.column1);
                    if (null != set.key.column)
                        setColumn.AddRange(set.key.column.Select(q => q.name));
                    List<string> collectedKeyColumns = new List<string>();
                    collectedKeyColumns.AddRange(cid.Items.OfType<keyproperty>().Select(q => q.column1 ?? q.name));
                    List<string> nonOverlappingColumns;
                    if (CheckOverlapping(setColumn, collectedKeyColumns, out nonOverlappingColumns))
                    {
                        setToRemove.Add(set);
                        entitiesToRemove.Add(collectedEntity.name);
                        map map = new map();
                        map.name = set.name;
                        map.table = set.table;
                        map.key = set.key;
                        var meta = context.GetTableMetaData(collectedEntity.schema, collectedEntity.table ?? collectedEntity.name);
                        if (nonOverlappingColumns.Count == 1)
                        {
                            map.Item = new index() { column1 = nonOverlappingColumns[0], type = typeConverter.GetNHType(meta.GetColumnMetadata(nonOverlappingColumns[0])) };
                        }
                        else
                        {
                            compositeindex ci = new compositeindex();
                            ci.@class = context.NamingStrategy.GetClassNameForComponentKey(map.name??map.table);
                            ci.Items = nonOverlappingColumns.Select(q => new keyproperty() {
                                    name = context.NamingStrategy.GetPropertyNameFromColumnName(q)
                                   ,column1 = context.NamingStrategy.GetPropertyNameFromColumnName(q) != q ? q : null
                                   ,type1=typeConverter.GetNHType(meta.GetColumnMetadata(q))
                                   ,length = meta.GetColumnMetadata(q).ColumnSize == 0 ? null:meta.GetColumnMetadata(q).ColumnSize.ToString()
                            }).ToArray();
                            map.Item = ci;
                        }
                        property[] props = collectedEntity.Items.OfType<property>().ToArray();
                        if (props.Length == 1)
                        {
                            //use an element
                            element e = new element();
                            e.column = props[0].column ?? props[0].name;
                            e.type1 = props[0].type1;
                            e.type = props[0].type;
                            e.precision = props[0].precision;
                            e.length = props[0].length;
                            map.Item1 = e;
                        }
                        else
                        {
                            //use a composite element
                            compositeelement ce = new compositeelement();
                            ce.@class = context.NamingStrategy.GetClassNameForCollectionComponent(collectedEntity.table??collectedEntity.name);
                            ce.Items = context.Model.GetPropertyOfEntity(collectedEntity.name);
                            map.Item1 = ce;
                        }
                        context.Model.AddCollectionToEntity(entity.name, map);
                    }
                }
            }
        }

        private bool CheckOverlapping(List<string> setColumn, List<string> collectedKeyColumns, out List<string> nonOverlappingColumns)
        {
            bool overlap = false;
            nonOverlappingColumns = new List<string>();
            HashSet<string> coll = new HashSet<string>();
            foreach (var s in collectedKeyColumns)
                coll.Add(s);
            foreach (var s in setColumn)
            {
                if (coll.Contains(s))
                {
                    overlap = true;
                    coll.Remove(s);
                }
            }
            foreach (var s in coll)
                nonOverlappingColumns.Add(s);
            return overlap && nonOverlappingColumns.Count>0;
        }
    }
}
