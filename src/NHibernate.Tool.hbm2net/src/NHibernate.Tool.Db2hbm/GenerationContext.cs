using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect.Schema;
using System.Data.Common;
using cfg;
using System.Data;

namespace NHibernate.Tool.Db2hbm
{
    ///<author>
    /// Felice Pollano (felice@felicepollano.com)
    ///</author>
    public class GenerationContext
    {
        public GenerationContext()
        {
            FilteredTables = new List<DataRow>();
        }
        Dictionary<string, object> items = new Dictionary<string,object>();
        public object this[string itemname]
        {
            get {
                object ret;
                items.TryGetValue(itemname, out ret);
                return ret;
            }
            set
            {
                items[itemname] = value;
            }

        }
        public List <DataRow> FilteredTables { get; set; }
        public db2hbmconf Configuration { get; set; }
        public IMappingModel Model { get; set; }
        public IDataBaseSchema Schema { get; set; }
        public NHibernate.Dialect.Dialect Dialect { get; set; }
        public DbConnection Connection { get; set; }
        public INamingStrategy NamingStrategy { get; set; }
        public ITableExceptions TableExceptions { get; set; }
        Dictionary<string, ITableMetadata> tableMetaData = new Dictionary<string, ITableMetadata>();
        Dictionary<string, List<ITableMetadata>> tableMetadataByName = new Dictionary<string, List<ITableMetadata>>();
        public void StoreTableMetaData(string catalog, string schema, string name, ITableMetadata metas)
        {
            tableMetaData[GetKey(catalog, schema, name)] = metas;
            if (!tableMetadataByName.ContainsKey(name))
                tableMetadataByName[name] = new List<ITableMetadata>();
            tableMetadataByName[name].Add(metas);
        }
        public ITableMetadata GetTableMetaData(string schema,string name)
        {
            List<ITableMetadata> list;
            if (tableMetadataByName.TryGetValue(name, out list))
            {
                if (list.Count == 1)
                {
                    return list[0];
                }
                else
                {
                    var candidates =  list.Where(q => string.Compare(q.Schema, schema, true) == 0);
                    if (candidates.Count() > 1)
                    {
                        throw new Exception("Ambiguous table name:" + name + "." + schema);
                    }
                    return candidates.FirstOrDefault();
                }
            }
            return null;
        }
        public ITableMetadata GetTableMetaData(string catalog, string schema, string name)
        {
            ITableMetadata res = null;
            tableMetaData.TryGetValue(GetKey(catalog, schema, name), out res);
            return res;
        }
        private string GetKey(string catalog, string schema, string name)
        {
            return string.Join(".", new string[]{catalog ?? "", schema ?? "", name});
        }
    }
}
