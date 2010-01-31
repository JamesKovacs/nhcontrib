using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;

namespace NHibernate.Tool.Db2hbm
{
    public class ManyToManyStrategy:KeyAwareMetadataStrategy
    {
        public ILog logger { set; protected get; }
        GenerationContext context;
        public TypeConverter typeConverter { set; protected get; }
        protected override void OnProcess(GenerationContext context)
        {
            this.context = context;
            foreach (var e in context.Model.GetEntities())
            {
                logger.Debug(string.Format("{0} working on:", GetType().Name) + e.table);
                if (IsManyToManyJoin(e))
                {
                    var kc = GetKeyColumns(context.GetTableMetaData(e.schema, e.name));
                    bool hasbag = false;
                    if (kc.Length == 2)//bag: key spawn two columns
                    {
                        hasbag = true;
                        manytoone[] refs = context.Model.GetManyToOnesOfEntity(e.name);
                        for( int i=0;i<refs.Length;++i )
                        {
                            int other = i == 0?1:0;
                            set set = new set();
                            set.table = e.table;
                            set.schema = e.schema;
                            set.catalog = e.catalog;
                            set.name = context.NamingStrategy.GetNameForCollection(refs[other].@class,0);
                            set.key = new key() { column1 = refs[i].column };
                            set.Item = new manytomany() 
                                { 
                                    @class = refs[other].@class
                                    ,Items = new column[]{ new column(){ name = refs[other].column}}
                                };
                            context.Model.AddCollectionToEntity(refs[i].@class,set);
                            logger.Warn(string.Format("Collection {0} on entity {1} is a many to many, need 'inverse' to be specified?",set.name,refs[i].@class));
                        }
                    }
                    else 
                    if( kc.Length==1 ) //idbag: a single column with the pair id.
                    {
                        hasbag = true;
                        manytoone[] refs = context.Model.GetManyToOnesOfEntity(e.name);
                        for (int i = 0; i < refs.Length; ++i)
                        {
                            int other = i == 0 ? 1 : 0;
                            idbag idbag = new idbag();
                            idbag.collectionid = new collectionid()
                                    {
                                        column1 = kc[0].Name
                                        ,
                                        type = typeConverter.GetNHType(kc[0])
                                        ,
                                        length = kc[0].ColumnSize > 0 ? kc[0].ColumnSize.ToString() : null
                                        ,
                                        generator = new generator() { @class = "native" }
                                    };
                            logger.Warn(string.Format("IdBag collection {0} on entity {1}: collection key generator defaults to native",idbag.name,refs[i].@class));
                            idbag.table = e.table;
                            idbag.schema = e.schema;
                            idbag.catalog = e.catalog;
                            idbag.name = context.NamingStrategy.GetNameForCollection(refs[other].@class, 0);
                            idbag.key = new key() { column1 = refs[i].column };
                            idbag.Item = new manytomany()
                            {
                                @class = refs[other].@class
                                ,
                                Items = new column[] { new column() { name = refs[other].column } }
                            };
                            context.Model.AddCollectionToEntity(refs[i].@class, idbag);
                            logger.Warn(string.Format("Collection {0} on entity {1} is a many to many, need 'inverse' to be specified?", idbag.name, refs[i].@class));
                        }
                    }
                    if (hasbag)
                    {
                        RemoveCurrentCollections(e);
                        context.Model.RemoveEntity(e.name);
                    }
                }
            }
        }

        private void RemoveCurrentCollections(@class e)
        {
            var mtoones = context.Model.GetManyToOnesOfEntity(e.name);
            foreach (var c in mtoones)
            {
                var collections = context.Model.GetCollectionsOfEntity(c.@class);
                foreach (var toremove in collections)
                {
                    if (IsOf(e, toremove))
                    {
                        context.Model.RemoveCollectionFromEntity(c.@class, toremove);
                    }
                }
            }
        }

        private bool IsOf(@class e, object toremove)
        {
            PropertyInfo pi = toremove.GetType().GetProperty("Item");
            if (null != pi)
            {
                onetomany theclass = pi.GetValue(toremove, null) as onetomany;
                return theclass != null && 0 == string.Compare(theclass.@class, e.name);
            }
            return false;
        }

        private bool IsManyToManyJoin(@class e)
        {
            // no properties and two many to ones is a many to many
            var props = context.Model.GetPropertyOfEntity(e.name);
            var mtoones = context.Model.GetManyToOnesOfEntity(e.name);
            return props.Length == 0 && mtoones.Length == 2;
        }
    }
}
