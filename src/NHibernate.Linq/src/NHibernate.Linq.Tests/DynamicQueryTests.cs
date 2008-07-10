using System;
using NUnit.Framework;
using NHibernate.Linq.Tests.Entities;
using System.Linq;
using System.Linq.Dynamic;

namespace NHibernate.Linq.Tests
{
    [TestFixture]
    public class DynamicQueryTests : BaseTest
    {
        protected override ISession CreateSession()
        {
            return GlobalSetup.CreateSession();
        }

        [Test]
        public void CanQueryWithDynamicOrderBy()
        {
            var query = from user in session.Linq<User>()
                        select user;

            //dynamic orderby clause
            query = query.OrderBy("RegisteredAt");

            var list = query.ToList();

            //assert list was returned in order
            DateTime previousDate = DateTime.MinValue;
            list.Each(delegate(User user)
            {
                Assert.IsTrue(previousDate <= user.RegisteredAt);
                previousDate = user.RegisteredAt;
            });
        }
    }
}
