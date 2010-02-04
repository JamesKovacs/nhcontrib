using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using cfg;
using System.Data;
using NHibernate.Dialect.Schema;

namespace NHibernate.Tool.Db2hbm
{
    
    public class TableEnumerator:IEnumerable<DataRow>
    {
        public static db2hbmconf Configuration { get; set; }
        static TableEnumerator instance;
        List<Matcher> Include = new List<Matcher>();
        List<Matcher> Exclude = new List<Matcher>();
        private TableEnumerator()
        {
            if (null != Configuration.tablefilter)
            {
                if (null != Configuration.tablefilter.exclude)
                {
                    Array.ForEach(Configuration.tablefilter.exclude, c=> Exclude.Add(new Matcher(c.table,c.catalog,c.schema)));
                }
                if (null != Configuration.tablefilter.include)
                {
                    Array.ForEach(Configuration.tablefilter.include, c => Include.Add(new Matcher(c.table, c.catalog, c.schema)));
                }
            }
        }
        protected IDataBaseSchema Schema { get; set; }
        public static TableEnumerator GetInstance(IDataBaseSchema schema)
        {
            
                if (null == Configuration)
                    throw new InvalidOperationException("Configuration not initialized");
                if (null == instance)
                    instance = new TableEnumerator();
                instance.Schema = schema;
                return instance; 
            
        }
        class Matcher
        {
            Regex tableRx;
            Regex catalogRx;
            Regex schemaRx;
            public Matcher(string tableregex,string catalogregex,string schemaregex)
            {
                if (null != tableregex)
                    tableRx = new Regex(tableregex, RegexOptions.Compiled);
                if (null != catalogregex)
                    catalogRx = new Regex(catalogregex, RegexOptions.Compiled);
                if (null != schemaregex)
                    schemaRx = new Regex(schemaregex, RegexOptions.Compiled);
            }

            internal bool Match(ITableMetadata meta)
            {
                return (null == catalogRx || catalogRx.IsProperMatch(meta.Catalog ?? ""))
                        &&
                        (null == schemaRx || schemaRx.IsProperMatch(meta.Schema ?? ""))
                        &&
                        (null == tableRx || tableRx.IsProperMatch(meta.Name ?? ""));

            }
        }

        #region IEnumerable<DataRow> Members

        public IEnumerator<DataRow> GetEnumerator()
        {
            foreach (DataRow t in Schema.GetTables(null, null, null, new string[0]).Rows)
            {
                var meta = Schema.GetTableMetadata(t, true);
                if( Match(meta) )
                    yield return t;
            }
        }
         #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<DataRow>).GetEnumerator();
        }

        #endregion
        private bool Match(ITableMetadata meta)
        {
            bool included = false;
            foreach (Matcher m in Include)
            {
                if (m.Match(meta))
                {
                    included = true;
                    break;
                }
            }
            foreach (Matcher m in Exclude)
            {
                if (m.Match(meta))
                {
                    included = false;
                    break;
                }
            }
            return included;
        }

       
    }
    static class RxExtension
    {
        public static bool IsProperMatch(this Regex r, string input)
        {
            Match m = r.Match(input);
            if (m.Success)
                return m.Value == input;
            return false;
        }
    }
}
