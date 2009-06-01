using System.Collections.Specialized;
using NHibernate.Dialect.Function;
using NUnit.Framework;

namespace NHibernate.JetDriver.Tests 
{
    [TestFixture]
    public class JetDialectFixture 
    {
        [Test]
        public void NH1181_SupportHqlUpper()
        {
            JetDialect dialect = new JetDialect();
            ISQLFunction func;
            Assert.IsTrue(dialect.Functions.TryGetValue("upper", out func));

            StringCollection sqlFuncParams = new StringCollection();
            sqlFuncParams.Add("foo");
            string sqlString = func.Render(sqlFuncParams, null).ToString();
            Assert.AreEqual("ucase(foo)", sqlString);
        }

        [Test]
        public void NH1181_SupportHqlLower()
        {
            JetDialect dialect = new JetDialect();
            ISQLFunction func;
            Assert.IsTrue(dialect.Functions.TryGetValue("lower", out func));

            StringCollection sqlFuncParams = new StringCollection();
            sqlFuncParams.Add("foo");
            string sqlString = func.Render(sqlFuncParams, null).ToString();
            Assert.AreEqual("lcase(foo)", sqlString);
        }
    }
}
