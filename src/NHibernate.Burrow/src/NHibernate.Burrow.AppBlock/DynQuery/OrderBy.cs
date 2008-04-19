using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Util;

namespace NHibernate.Burrow.AppBlock.DynQuery {
    /// <summary>
    /// The class that represent the "order by" clause of a HQL/SQL.
    /// </summary>
    /// <remarks>
    /// The syntax is cheked when the HQL/SQL will be parsed.
    /// </remarks>
    [Serializable]
    public class OrderBy : IDynClause {
        private readonly Dictionary<string, string> order = new Dictionary<string, string>();

        #region IDynClause Members

        /// <summary>
        /// The query clause.
        /// </summary>
        public string Clause {
            get { return (!HasMembers) ? string.Empty : string.Format("order by {0}", Expression); }
        }

        /// <summary>
        /// The query part.
        /// </summary>
        public string Expression {
            get { return GetExpression(); }
        }

        /// <summary>
        /// The clause has some meber or not?
        /// </summary>
        public bool HasMembers {
            get { return order.Count > 0; }
        }

        #endregion

        /// <summary>
        /// Add a property path to the "order by" clause.
        /// </summary>
        /// <param name="propertyPath">The property path (ex: f.Name).</param>
        /// <param name="isDescending">True if the direction is DESCENDING.</param>
        /// <returns>The <see cref="OrderBy"/> it self.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="propertyPath"/> is null or empty.</exception>
        public OrderBy Add(string propertyPath, bool isDescending) {
            if (string.IsNullOrEmpty(propertyPath))
                throw new ArgumentNullException("propertyPath");
            string pp = propertyPath.Trim();
            if (pp.Length == 0)
                throw new ArgumentNullException("propertyPath");
            order[pp] = (isDescending ? "desc" : null);
            return this;
        }

        /// <summary>
        /// Add a property path to the order by clause.
        /// </summary>
        /// <param name="propertyPath">The property path (ex: f.Name).</param>
        /// <returns>The <see cref="OrderBy"/> it self.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="propertyPath"/> is null or empty.</exception>
        public OrderBy Add(string propertyPath) {
            return Add(propertyPath, false);
        }

        private string GetExpression() {
            if (!HasMembers)
                return string.Empty;
            StringBuilder clause = new StringBuilder(order.Count*32 + 9);

            IEnumerator<KeyValuePair<string, string>> iter = order.GetEnumerator();
            iter.MoveNext();
            clause.Append(string.Format("{0} {1}", iter.Current.Key, iter.Current.Value).Trim());
            while (iter.MoveNext())
                clause.Append(StringHelper.CommaSpace).Append(
                    string.Format("{0} {1}", iter.Current.Key, iter.Current.Value).Trim());

            return clause.ToString();
        }
    }
}