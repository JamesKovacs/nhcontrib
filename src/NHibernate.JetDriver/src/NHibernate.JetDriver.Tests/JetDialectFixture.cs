using System.Collections.Specialized;
using NHibernate.Dialect.Function;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NHibernate.JetDriver.Tests 
{
    [TestFixture]
    public class JetDialectFixture : JetTestBase
    {
        [Test]
        public void NH1181_SupportHqlUpper()
        {
            var dialect = new JetDialect();
            ISQLFunction func;
            Assert.IsTrue(dialect.Functions.TryGetValue("upper", out func));

            var sqlFuncParams = new StringCollection { "foo" };
            var sqlString = func.Render(sqlFuncParams, null).ToString();

            Assert.That(sqlString, Is.EqualTo("ucase(foo)"));
        }

        [Test]
        public void NH1181_SupportHqlLower()
        {
            var dialect = new JetDialect();
            ISQLFunction func;
            Assert.IsTrue(dialect.Functions.TryGetValue("lower", out func));

            var sqlFuncParams = new StringCollection { "foo" };
            var sqlString = func.Render(sqlFuncParams, null).ToString();

            Assert.That(sqlString, Is.EqualTo("lcase(foo)"));
        }
    }
}
