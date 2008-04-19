using System;
using System.Text;

namespace NHibernate.Burrow.AppBlock.DynQuery
{
    public enum LogicalOperator
    {
        Null,
        And,
        Or,
        Not
    }

    [Serializable]
    public class LogicalExpression : IQueryPart
    {
        private readonly string expression;
        private readonly LogicalOperator loperator;

        internal LogicalExpression(LogicalOperator loperator, string expression)
        {
            this.loperator = loperator;
            this.expression = expression.Trim();
        }

        public LogicalExpression(string expression) : this(LogicalOperator.Null, expression) {}

        #region IQueryPart Members

        /// <summary>
        /// The query complete clause.
        /// </summary>
        public string Clause
        {
            get
            {
                StringBuilder result = new StringBuilder(200);
                switch (loperator)
                {
                    case LogicalOperator.And:
                        result.Append(" and ").Append(Expression);
                        break;
                    case LogicalOperator.Or:
                        result.Append(" or ").Append(Expression);
                        break;
                    case LogicalOperator.Not:
                        result.Append(" not ").Append(Expression);
                        break;
                    case LogicalOperator.Null:
                    default:
                        result.Append(Expression);
                        break;
                }
                return result.ToString();
            }
        }

        /// <summary>
        /// The query part.
        /// </summary>
        public string Expression
        {
            get { return string.Format("({0})", expression); }
        }

        #endregion
    }
}