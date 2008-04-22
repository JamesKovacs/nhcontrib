using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.SqlCommand;

namespace NHibernate.Burrow.AppBlock.Criterions
{
    /// <summary>
    /// An <see cref="ICriterion"/> that represents an "equal" or "null" constraint dependig on
    /// the value of the property.
    /// </summary>
    /// <seealso cref="Restrictions.Eq(string,object)"/>
    /// <seealso cref="NullExpression"/>
    public class EqOrNullExpression : AbstractCriterion
    {
        private readonly ICriterion realCriterion;

        /// <summary>
        /// Initialize a new instance of the <see cref="EqOrNullExpression" /> class for a named
        /// Property and its value.
        /// </summary>
        /// <param name="propertyName">The name of the Property in the class.</param>
        /// <param name="value">The value for the Property.</param>
        public EqOrNullExpression(string propertyName, object value) : this(propertyName, value, false) {}

        public EqOrNullExpression(string propertyName, object value, bool ignoreCase)
        {
            if (value == null)
            {
                realCriterion = new NullExpression(propertyName);
            }
            else
            {
                realCriterion = new SimpleExpression(propertyName, value, " = ", ignoreCase);
            }
        }

        public override string ToString()
        {
            return realCriterion.ToString();
        }

        public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return realCriterion.GetTypedValues(criteria, criteriaQuery);
        }

        public override SqlString ToSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery,
                                              IDictionary<string, IFilter> enabledFilters)
        {
            return realCriterion.ToSqlString(criteria, criteriaQuery, enabledFilters);
        }

    	public override IProjection[] GetProjections() {
    		return realCriterion.GetProjections();
    	}
    }
}