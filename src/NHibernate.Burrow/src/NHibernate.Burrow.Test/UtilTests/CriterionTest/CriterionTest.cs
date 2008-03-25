using System.Collections;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Loader.Criteria;
using NHibernate.SqlCommand;
using NHibernate.Util;
using NHibernate.Burrow.Util.Criterions;
using Criterion = NHibernate.Burrow.Util.Criterions.Criterion;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.UtilTests.CriterionTest
{
    [TestFixture]
    public class CriterionTest:TestCase
    {
        protected override IList Mappings
        {
            get { return new string[] { "UtilTests.CriterionTest.Simple.hbm.xml" }; }
        }

        [Test]
        public void EqOrNullTest()
        {
            using (ISession session = OpenSession())
            {
                CriteriaImpl criteria = (CriteriaImpl) session.CreateCriteria(typeof (Simple));
                CriteriaQueryTranslator criteriaQuery = new CriteriaQueryTranslator((ISessionFactoryImplementor) sessions, criteria, criteria.EntityOrClassName, "sql_alias");

                ICriterion exp = NHibernate.Burrow.Util.Criterions.Criterion.EqOrNull("Name", "foo");
                CollectionHelper.EmptyMapClass<string, IFilter> emptyMap = new CollectionHelper.EmptyMapClass<string, IFilter>();
                SqlString sqlString = exp.ToSqlString(criteria, criteriaQuery, emptyMap);

                string expectedSql = "sql_alias.Name = ?";

                Assert.AreEqual(expectedSql, sqlString.ToString());
                Assert.AreEqual(1, sqlString.GetParameterCount());

                exp = NHibernate.Burrow.Util.Criterions.Criterion.EqOrNull("Name", null);
                sqlString = exp.ToSqlString(criteria, criteriaQuery, emptyMap);

                expectedSql = "sql_alias.Name is null";

                Assert.AreEqual(expectedSql, sqlString.ToString());
                Assert.AreEqual(0, sqlString.GetParameterCount());

                // Check that the result is the same than using official Expression
                ICriterion orExpExpected = Expression.Or(Expression.IsNull("Name"), Expression.Eq("Name", "foo"));
                ICriterion orExpActual = Expression.Or(NHibernate.Burrow.Util.Criterions.Criterion.EqOrNull("Name", null), NHibernate.Burrow.Util.Criterions.Criterion.EqOrNull("Name", "foo"));

                SqlString sqlStringExpected = orExpExpected.ToSqlString(criteria, criteriaQuery, emptyMap);
                SqlString sqlStringActual = orExpActual.ToSqlString(criteria, criteriaQuery, emptyMap);
                Assert.AreEqual(sqlStringExpected.ToString(), sqlStringActual.ToString());
            }
        }
    }
}