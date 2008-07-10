using System;
using System.Linq;
using System.Linq.Expressions;

namespace NHibernate.Linq.Expressions
{
    public class QuerySourceExpression : NHibernateExpression
    {
        private readonly string _alias;
        private readonly IQueryable _query;

        public string Alias
        {
            get { return _alias; }
        }

        public IQueryable Query
        {
            get { return _query; }
        }

        public QuerySourceExpression(string alias, IQueryable query)
            : base(NHibernateExpressionType.QuerySource, query.GetType())
        {
            _alias = alias;
            _query = query;
        }

        public override string ToString()
        {
            if (!String.IsNullOrEmpty(Alias))
                return Alias;

            return base.ToString();
        }
    }
}
