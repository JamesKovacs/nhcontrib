using System;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Util;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests
{
    [TestFixture]
    public class WrapUrlFixture
    {
        [Test]
        public void WrapTest()
        {
            new BurrowFramework().InitWorkSpace();
            new BurrowFramework().CurrentConversation.SpanWithPostBacks();
            string wrapped = new WebUtil().WrapUrlWithConversationInfo("http://test.com");
            Assert.IsTrue(wrapped.Contains("http://test.com?_NHibernate.Burrow.ConversationId_="));
            wrapped = new WebUtil().WrapUrlWithConversationInfo("http://test.com?t=1");
            Assert.IsTrue(wrapped.Contains("http://test.com?t=1&_NHibernate.Burrow.ConversationId_="));
            new BurrowFramework().CurrentConversation.FinishSpan();
            new BurrowFramework().CloseWorkSpace();
        }

      
    }
}