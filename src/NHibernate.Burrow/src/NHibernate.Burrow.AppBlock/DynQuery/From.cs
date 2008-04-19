using System;
using System.Text;

namespace NHibernate.Burrow.AppBlock.DynQuery {
    /// <summary>
    /// The class that represent the "from" clause of a HQL/SQL.
    /// </summary>
    /// <remarks>
    /// The syntax is cheked when the HQL/SQL will be parsed.
    /// </remarks>
    [Serializable]
    public class From : IDynClause {
        private readonly string partialClause;
        private OrderBy orderBy;

        private Where where;

        /// <summary>
        /// Create a new instance of <see cref="From"/>.
        /// </summary>
        /// <param name="partialClause">The "from" clause, of the query, without the "from" word.</param>
        public From(string partialClause) {
            if (string.IsNullOrEmpty(partialClause) || partialClause.Trim().Length == 0)
                throw new ArgumentNullException("partialClause");
            this.partialClause = partialClause.Trim();
        }

        #region IDynClause Members

        /// <summary>
        /// The query clause.
        /// </summary>
        public string Clause {
            get {
                StringBuilder result = new StringBuilder().Append(Expression);
                if (Where().HasMembers)
                    result.Append(' ').Append(Where().Clause);
                if (OrderBy().HasMembers)
                    result.Append(' ').Append(OrderBy().Clause);
                return result.ToString();
            }
        }

        /// <summary>
        /// The query part.
        /// </summary>
        public string Expression {
            get { return string.Format("from {0}", partialClause); }
        }

        /// <summary>
        /// The clause has some meber or not?
        /// </summary>
        public bool HasMembers {
            get { return partialClause.Trim().Length > 0; }
        }

        #endregion

        public Where Where() {
            if (where == null)
                where = new Where();
            return where;
        }

        public Where Where(string expression) {
            if (where != null)
                throw new NotSupportedException(
                    string.Format("Can't override the 'where' clause; original 'where':{0}", where.Clause));
            where = new Where(expression);
            return where;
        }

        public Where WhereNot() {
            if (where != null)
                throw new NotSupportedException(
                    string.Format("Can't override the 'where' clause; original 'where':{0}", where.Clause));
            where = new Where(true);
            return where;
        }

        public void SetWhere(Where whereClause) {
            if (whereClause == null)
                throw new ArgumentNullException("whereClause");
            where = whereClause;
        }

        public OrderBy OrderBy() {
            if (orderBy == null)
                orderBy = new OrderBy();
            return orderBy;
        }

        public OrderBy OrderBy(string propertyPath) {
            return OrderBy(propertyPath, false);
        }

        public OrderBy OrderBy(string propertyPath, bool isDescending) {
            return OrderBy().Add(propertyPath, isDescending);
        }

        public void SetOrderBy(OrderBy orderClause) {
            if (orderClause == null)
                throw new ArgumentNullException("orderClause");
            orderBy = orderClause;
        }

        public From FromWhereClause() {
            From result = new From(partialClause);
            if (where != null)
                result.SetWhere(where.Clone());
            return result;
        }
    }
}