using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using uNhAddIns.Inflector;
using System.Reflection;

namespace NHibernate.Tool.Db2hbm
{
    class DefaultNamingStrategy:INamingStrategy
    {
        public DefaultNamingStrategy()
        {

        }

        IInflector inflector;

        private IInflector Inflector
        {
            get {
                if (null == inflector)
                {
                    var t = System.Type.GetType(InferType(Language));
                    if (null != t)
                    {
                        inflector = Activator.CreateInstance(t) as IInflector;
                    }
                }
                return inflector;
            }
        }

      

        private string InferType(string language)
        {
            return string.Format("uNhAddIns.Inflector.{0}Inflector,{1}", language, Assembly.GetExecutingAssembly().FullName);
        }
        public string Irregulars { get; set; }
        public string Language { get; set; }

        #region INamingStrategy Members

        public string EntityNameFromTableName(string tableName)
        {
            if (null != Inflector)
            {
                return inflector.Singularize(tableName);
            }
            return tableName;
        }

        public string PropertyNameFromColumnName(string columnName)
        {
            return columnName;
        }

        #endregion

        #region INamingStrategy Members


        public string ClassNameForComponentKey(string entityName)
        {
            return entityName + "Key";
        }

        public string PropertyNameForComponentKey(string entityName, string componentClass)
        {
            return "Id";
        }

        #endregion
    }
}
