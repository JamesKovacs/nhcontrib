using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect.Schema;
using System.Data.Common;
using cfg;

namespace NHibernate.Tool.Db2hbm
{
    public class GenerationContext
    {
        public db2hbmconf Configuration { get; set; }
        public IMappingModel Model { get; set; }
        public IDataBaseSchema Schema { get; set; }
        public NHibernate.Dialect.Dialect Dialect { get; set; }
        public DbConnection Connection { get; set; }
        public INamingStrategy NamingStrategy { get; set; }
        public ITableExceptions TableExceptions { get; set; }
    }
}
