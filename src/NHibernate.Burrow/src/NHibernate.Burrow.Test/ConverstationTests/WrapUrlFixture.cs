using System;
using NHibernate.Burrow.Util;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests {
    [TestFixture]
    public class WrapUrlFixture  {
        [Test]
        public void WrapWithoutSpanTest() {
            try {
                new Facade().InitializeDomain();
                new WebUtil().WrapUrlWithConversationInfo("http://test.com");
                Assert.Fail("failed to throw IncorrectConversationSpanStatusException");

            }catch(Exceptions.IncorrectConversationSpanStatusException) {
                
            }finally {
                new Facade().CloseDomain();
            }
        }
        [Test]
        public void WrapTest() {
            new Facade().InitializeDomain();
            new Facade().CurrentConversation.SpanWithPostBacks();
            string wrapped = new WebUtil().WrapUrlWithConversationInfo("http://test.com");
            Assert.IsTrue(wrapped.Contains( "http://test.com?NHibernate.Burrow.ConversationId="));
            wrapped = new WebUtil().WrapUrlWithConversationInfo("http://test.com?t=1");
            Assert.IsTrue(wrapped.Contains( "http://test.com?t=1&NHibernate.Burrow.ConversationId="));
            Console.WriteLine(wrapped);
            new Facade().CurrentConversation.FinishSpan();
            new Facade().CloseDomain();
        }
    }
}