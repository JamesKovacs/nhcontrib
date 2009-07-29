using System.Collections.Generic;
using System.Collections.Specialized;
using NHibernate.Dialect.Function;
using NHibernate.JetDriver.Tests.Entities;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NHibernate.JetDriver.Tests 
{
    [TestFixture]
    public class JetDialectFixture : JetTestBase
    {
        protected override IList<System.Type> EntityTypes
        {
            get { return new[] { typeof(Foo) }; }
        }

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

        [Test]
        public void NHCD25_Supporting_Counter_DataType_For_Identity_Column()
        {
            var script = string.Empty;
            var exporter = new SchemaExport(base.Configuration);
            exporter.Execute(s => script = s, false, false);

            Assert.That(script, Is.Not.Empty);
            Assert.That(script.Contains("ModuleId  COUNTER"), Is.True, "DataType of ModuleId (PK) is of type COUNTER");
        }

        [Test]
        public void NHCD27_Long_Type_Is_Mapped_To_Real_DataType()
        {
            var script = string.Empty;
            var exporter = new SchemaExport(base.Configuration);
            exporter.Execute(s => script = s, false, false);

            Assert.That(script, Is.Not.Empty);
            Assert.That(script.Contains("ModuleValue REAL"), Is.True, "Long dataType is mapped to REAL");            
        }

        [Test]
        public void NHCD14_Parameter_Identifier_Placeholder()
        {
            var script = string.Empty;
            var exporter = new SchemaExport(base.Configuration);
            exporter.Execute(s => script = s, false, false);

            Assert.That(script, Is.Not.Empty);
            Assert.That(script.Contains("ShortName TEXT(100)"), Is.True, "ShortName length is 255");
            Assert.That(script.Contains("LongName MEMO"), Is.True, "LongName length is larger than 255, MEMO type is used");
            Assert.That(script.Contains("Description MEMO"), Is.True, "Description length is larger than 255, MEMO type is used");
        }
    }
}
