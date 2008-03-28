using System.Collections;
using System.Collections.Generic;
using NHibernate.Burrow.Util.Hql.Gold;
using NHibernate.Cfg;
using NUnit.Framework;

namespace NHibernate.Burrow.Util.Test.Hql
{
    [TestFixture]
    public class QueryTranslatorFixture : TestCase
    {
        #region Setup/Teardown

        [SetUp]
        public void Init()
        {
            // Verify correct query translator is being used
            Assert.AreEqual(typeof (QueryTranslatorFactory), sessions.Settings.QueryTranslatorFactory.GetType());
        }

        #endregion

        /* Created the AstBuilderFixture, continued later
		[Test]
		public void BuildTree()
		{
			QueryTranslator translator = BuildTranslator("from Object");
		}
		*/

        private QueryTranslator BuildTranslator(string query)
        {
            base.BuildSessionFactory();
            return new QueryTranslator("query", query, new Dictionary<string, IFilter>(), null);
        }

        protected override void Configure(Configuration configuration)
        {
            configuration.Properties.Add(Environment.QueryTranslator,
                                         typeof (QueryTranslatorFactory).AssemblyQualifiedName);
            base.Configure(configuration);
        }

        /// <summary>
        /// Mapping files used in the TestCase
        /// </summary>
        protected override IList Mappings
        {
            get { return new string[0]; }
        }
    }
}