using System;
using System.Collections.Generic;
using NUnit.Framework;
using NHibernate.Linq.Tests.Entities;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace NHibernate.Linq.Tests
{
    [TestFixture]
    public class MethodCallTests : BaseTest
    {
        protected override ISession CreateSession()
        {
            return GlobalSetup.CreateSession();
        }

        [Test]
        public void CanExecuteAny()
        {
            bool result = session.Linq<User>().Any();
            Assert.IsTrue(result);
        }

        [Test]
        public void CanExecuteAnyWithArguments()
        {
            bool result = session.Linq<User>().Any(u => u.Name == "user-does-not-exist");
            Assert.IsFalse(result);
        }
    }
}
