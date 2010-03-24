using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Hql.Ast.ANTLR;

namespace NHibernate.Tool.Db2hbm
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=false)]
    public class BooDecoratorAttribute:Attribute
    {
        public BooDecoratorAttribute()
        {
           
            
        }
    }
}
