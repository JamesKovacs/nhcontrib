using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using log4net.Config;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Iesi.Collections;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Connection;
using NHibernate.ByteCode.LinFu;
using NHibernate.Tool.hbm2ddl;
using System.Xml.Schema;
using System.Xml;
using NHibernate.Dialect.Schema;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace NHibernate.Tool.hbm2net.Tests
{

    [TestFixture]
    public class Experiments
    {
        [Test,Ignore]
        public void InquireSchema()
        {
            Dialect.MsSql2005Dialect dialect = new MsSql2005Dialect();
            DbConnection conn = new SqlConnection(@"");
            conn.Open();

            DatabaseMetadata meta = new DatabaseMetadata(conn, dialect);


            
            IDataBaseSchema schema = dialect.GetDataBaseSchema(conn);
            var dt = schema.GetTables(null, null, null, new string[0]);
            var cols = schema.GetColumns(null, null, null, null);
            var keys = schema.GetForeignKeys(null, null, null);
            foreach (DataRow r in dt.Rows)
            {
                var tableMeta = schema.GetTableMetadata(r, true);
                Console.WriteLine(string.Format("Table {2}:[{0}].[{1}]",tableMeta.Schema,tableMeta.Name,tableMeta.Catalog));
                ITableMetadata tm = meta.GetTableMetadata(tableMeta.Name, tableMeta.Schema, tableMeta.Catalog, false);
                IColumnMetadata col = tm.GetColumnMetadata(cols.Rows[0].ItemArray[2] as string);
            }
        }
    }
}
