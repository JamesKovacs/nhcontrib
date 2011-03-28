using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Reflection;
using System.Collections.Generic;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using NHibernate.JetDriver.Tests.Entities;

namespace NHibernate.JetDriver.Tests
{
    [TestFixture]
    public class JetCommandFixture : JetTestBase
    {
        public JetCommandFixture() : base(true)
        {
        }

        protected override IList<System.Type> EntityTypes
        {
            get { return new[] { typeof(DecimalEntity) }; }
        }

        [Test]
        public void Decimal_Values_Are_Not_Truncated_Once_Saved()
        {
            using (new ContextCultureSwitch(new CultureInfo("sv-SE")))
            {
                var entity = new DecimalEntity
                {
                    SimpleDecimal = 1.1m,
                    NullableDecimal = 1.3m,
                };

                var session = SessionFactory.OpenSession();

                session.Save(entity);
                session.Flush();
                session.Clear();

                var readback = session.Load<DecimalEntity>(entity.Id);

                Assert.IsNotNull(readback);
                Assert.AreEqual((decimal)1.1, readback.SimpleDecimal);
                Assert.AreEqual((decimal?)1.3, readback.NullableDecimal);
            }
        }

        [Test]
        public void Converts_Decimal_Parameter_To_Double()
        {
            using (var culture = new ContextCultureSwitch(new CultureInfo("sv-SE")))
            {
                JetDbCommand cmd = new JetDbCommand("SELECT * FROM [DecimalEntity] WHERE SimpleDecimal = @SimpleDecimal");
                OleDbParameter decimalParam = (OleDbParameter)cmd.CreateParameter();

                decimalParam.DbType = DbType.Decimal;
                decimalParam.ParameterName = "@SimpleDecimal";
                decimalParam.SourceColumn = "SimpleDecimal";
                decimalParam.Value = decimal.Parse("1,1", culture.Current); //Some culture use comma for decimal separator

                cmd.Parameters.Add(decimalParam);
                cmd.GetType().GetMethod("CheckParameters", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod).Invoke(cmd, null);

                IDataParameter parameter = (IDataParameter)cmd.Parameters[0];

                Assert.That(parameter.DbType, Is.EqualTo(DbType.Double));
                Assert.That(parameter.Value, Is.EqualTo(1.1m));
            }
        }

        [Test]
        public void NHCD32_Select_Statement_In_Cultures_With_Unconventional_AMPM_Designator_Will_Run()
        {
            using (new ContextCultureSwitch(new CultureInfo("fa-ir")))
            {
                JetDbCommand cmd = new JetDbCommand("SELECT * FROM [Employees] WHERE EmploymentDate > @DateParam");
                DateTime date = new DateTime(2000, 1, 1);
                OleDbParameter param = (OleDbParameter) cmd.CreateParameter();

                param.DbType = DbType.DateTime;
                param.ParameterName = "@DateParam";
                param.SourceColumn = "EmploymentDate";
                param.Value = date;

                cmd.Parameters.Add(param);
                cmd.GetType().GetMethod("CheckParameters", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod).Invoke(cmd, null);

                IDataParameter parameter = (IDataParameter) cmd.Parameters[0];

                Assert.That(parameter.Value, Is.TypeOf(typeof (string)));
                Assert.That(parameter.Value, Is.EqualTo(date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
            }
        }

        [Test]
        public void NHCD32_Two_Pass_Parameters_Of_Type_String_Are_Converted_To_Invariant_Date()
        {
            using (new ContextCultureSwitch(new CultureInfo("fa-ir")))
            {
                JetDbCommand cmd = new JetDbCommand("SELECT * FROM [Employees] WHERE EmploymentDate > @DateParam");
                OleDbParameter param = (OleDbParameter) cmd.CreateParameter();
                string dateString = "05/27/2009 02:16:32 ب.ظ.";

                param.DbType = DbType.DateTime;
                param.ParameterName = "@DateParam";
                param.SourceColumn = "EmploymentDate";
                param.Value = DateTime.Parse(dateString, CultureInfo.CurrentCulture);

                cmd.Parameters.Add(param);

                //First pass
                cmd.GetType()
                    .GetMethod("CheckParameters", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod)
                    .Invoke(cmd, null);

                //Second pass, dbtype is converted to string
                cmd.GetType()
                    .GetMethod("CheckParameters", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod)
                    .Invoke(cmd, null);

                var parameter = (IDataParameter)cmd.Parameters[0];

                Assert.That(parameter.Value, Is.TypeOf(typeof(string)));
                Assert.That(parameter.Value, Is.EqualTo("2009/05/27 14:16:32"));
            }
        }
    }
}