using System;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests {
    [TestFixture]
    public class WrapUrlFixture  {
        [Test]
        public void WrapWithoutSpanTest() {
            try {
                Facade.InitializeDomain();
                Facade.WrapUrlWithConversationInfo("http://test.com");
                Assert.Fail("failed to throw IncorrectConversationSpanStatusException");

            }catch(Exceptions.IncorrectConversationSpanStatusException) {
                
            }finally {
                Facade.CloseDomain();
            }
        }
        [Test]
        public void WrapTest() {
            Facade.InitializeDomain();
            Facade.CurrentConversation.SpanWithPostBacks();
            string wrapped = Facade.WrapUrlWithConversationInfo("http://test.com");
            Assert.IsTrue(wrapped.Contains( "http://test.com?NHibernate.Burrow.ConversationId=")); 
            wrapped = Facade.WrapUrlWithConversationInfo("http://test.com?t=1");
            Assert.IsTrue(wrapped.Contains( "http://test.com?t=1&NHibernate.Burrow.ConversationId="));
            Console.WriteLine(wrapped);
            Facade.CurrentConversation.FinishSpan();
            Facade.CloseDomain();
        }
    }
}