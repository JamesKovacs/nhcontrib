using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Linq.Expressions
{
    /// <summary>
    /// Extended node types for custom expressions
    /// </summary>
    public enum NHibernateExpressionType
    {
        QuerySource = 1000, //make sure these don't overlap with ExpressionType
        RootEntity,
        Entity,
        PropertyAccess,
        CollectionAccess
    }
}
