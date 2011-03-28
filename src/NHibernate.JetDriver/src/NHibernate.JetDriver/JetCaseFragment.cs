using System.Collections.Generic;
using System.Text;

using NHibernate.SqlCommand;
using NHibernate.Util;

namespace NHibernate.JetDriver
{
    /// <summary>
    /// Jet engine doesn't support CASE ... WHEN ... END syntax, but has a proprietary "Switch". 
    /// </summary>
    public class JetCaseFragment : CaseFragment
    {
        readonly List<string> caseStatements = new List<string>();

        public JetCaseFragment(Dialect.Dialect dialect)
            : base(dialect)
        {
        }

        public override CaseFragment AddWhenColumnNotNull(string alias, string columnName, string columnValue)
        {
            string key = alias + StringHelper.Dot + columnName + " is not null";

            caseStatements.Add(key + ", " + columnValue);
            return this;
        }

        public override string ToSqlStringFragment()
        {
            StringBuilder buf = new StringBuilder(cases.Count * 15 + 10);

            buf
                .Append("Switch(")
                .Append(string.Join(", ", caseStatements.ToArray()))
                .Append(" )");

            if (returnColumnName != null)
            {
                buf.Append(" as ")
                    .Append(returnColumnName);
            }

            return buf.ToString();
        }
    }
}
