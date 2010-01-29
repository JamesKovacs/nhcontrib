using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using uNhAddIns.Inflector;
using System.Reflection;

namespace NHibernate.Tool.Db2hbm
{
    public class DefaultNamingStrategy:INamingStrategy
    {
        public DefaultNamingStrategy()
        {
            Language = "English";
        }
        IInflector inflector;
        protected IInflector Inflector
        {
            get {
                if (null == inflector)
                {
                    if (InflectorType != null)
                    {
                        inflector = TypeFactory.Create<IInflector>(InflectorType);
                    }
                    else
                    {
                        var t = System.Type.GetType(InferType(Language));
                        if (null != t)
                        {
                            inflector = Activator.CreateInstance(t) as IInflector;
                        }
                        if (null == inflector)
                            inflector = new EnglishInflector(); // guarantee a reasonable default.
                    }
                }
                return inflector;
            }
        }
        public string InflectorType { get; set; }
        private string InferType(string language)
        {
            return string.Format("uNhAddIns.Inflector.{0}Inflector,{1}", language, Assembly.GetExecutingAssembly().FullName);
        }
        [BooDecorator]
        public string Customization { get; set; }
        public string Language { get; set; }
        #region INamingStrategy Members
        public virtual string GetEntityNameFromTableName(string tableName)
        {
            return Inflector.Pascalize(
                Inflector.Singularize(tableName.Trim('_'))
                );
        }
        public virtual string GetPropertyNameFromColumnName(string columnName)
        {
            return Inflector.Pascalize(columnName.Trim('_'));
        }
        public virtual string GetIdPropertyNameFromColumnName(string columnName)
        {
            return Inflector.Pascalize(columnName.Trim('_'));
        }
       
        public virtual string GetClassNameForComponentKey(string entityName)
        {
            return entityName + "Key";
        }
        public virtual string GetNameForComponentKey(string entityName, string componentClass)
        {
            return "Id";
        }
        public string GetNameForManyToOne(string referredEntity, string[] columnNames)
        {
            return referredEntity;
        }
        public string GetNameForCollection(string collectingClass, int progressive)
        {
            string cname = Inflector.Pluralize(collectingClass);
            if (progressive > 0)
                cname += progressive.ToString();
            return cname;
        }

        public string GetClassNameForCollectionComponent(string collectionTableName)
        {
            return Inflector.Singularize(collectionTableName);
        }

        #endregion
    }
}
