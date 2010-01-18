using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect.Schema;

namespace NHibernate.Tool.Db2hbm
{
    public class TypeConverter
    {
        cfg.db2hbmconf config;
        public TypeConverter(cfg.db2hbmconf config )
        {
            this.config = config;
        }
        public  string GetNHType(IColumnMetadata cInfo)
        {
            var comp = config.typemapping.Where(t => string.Compare(t.dbtype, cInfo.TypeName, true) == 0);
            foreach (var candidate in comp)
            {
                if (candidate.lengthSpecified)
                    if (candidate.length == cInfo.ColumnSize)
                        return candidate.nhtype;
            }
            if (comp.Count() > 0)
                return comp.First().nhtype;
            //logger.Warn(string.Format("No NHibernate type defined for dbtype:{0} len:{1}", cInfo.TypeName, cInfo.ColumnSize));
            return null;
        }
    }
}
