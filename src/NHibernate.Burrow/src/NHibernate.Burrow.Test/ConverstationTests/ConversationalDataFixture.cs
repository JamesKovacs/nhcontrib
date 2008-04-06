using System;
using System.Collections.Specialized;
using NHibernate.Burrow;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests {
    [TestFixture]
    public class ConversationalDataFixture  {
        readonly Facade f = new Facade();
        [Test]
        public void ConversationalDataPersistsTest() {
           
            f.InitializeDomain();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Single);
            ConversationalData<string> s = new ConversationalData<string>(ConversationalDataMode.Single, "sometext");
            Assert.AreEqual(0, i.Value);
            Assert.AreEqual("sometext", s.Value);
            i.Value = 1;
            s.Value = null;
            Assert.AreEqual(1, i.Value);
            Assert.IsNull(s.Value);
            f.CurrentConversation.SpanWithPostBacks();
            Guid cid = f.CurrentConversation.Id;

            f.CloseDomain();
            f.InitializeDomain(cid);
               
            Assert.AreEqual(1, i.Value);
            Assert.IsNull(s.Value);
            f.CurrentConversation.FinishSpan();
            f.CloseDomain();
            
        }

        [Test]
        public void ConversationalDataSpanBahaviorTest()
        {
            f.InitializeDomain();
            ConversationalData<int> single = new ConversationalData<int>(ConversationalDataMode.Single, 1);
            ConversationalData<int> singleTemp = new ConversationalData<int>(ConversationalDataMode.SingleTemp, 1);
            
            f.CurrentConversation.SpanWithPostBacks();
            Guid cid1 = f.CurrentConversation.Id;

            f.CloseDomain();
            f.InitializeDomain();
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            f.CloseDomain();
            f.InitializeDomain();
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            singleTemp.Value = 2;
            f.CurrentConversation.SpanWithPostBacks();
            Guid cid2 = f.CurrentConversation.Id;
            Assert.AreNotEqual(cid2, cid1);
            f.CloseDomain();
            f.InitializeDomain(cid1);
            Assert.AreEqual(1,single.Value);
            Assert.AreEqual(0, singleTemp.Value);
            f.CurrentConversation.FinishSpan();
            f.CloseDomain();
            f.InitializeDomain(cid2);
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            f.CurrentConversation.FinishSpan();
            f.CloseDomain();
        }

        private void ExpectConversationUnavailableException(ConversationalData<int> c) {
            try
            {
                c.Value.ToString();
                Assert.Fail("Failed to throw a ConversationUnavailableException.");
            }
            catch (ConversationUnavailableException) { }
        }

        [Test]
        public void SpanOverMultipleDomainContextTest() {
            f.InitializeDomain();
            f.CurrentConversation.SpanWithPostBacks();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Single);
            ConversationalData<string> s = new ConversationalData<string>(ConversationalDataMode.Single, "sometext");
            Guid gid = f.CurrentConversation.Id;
            i.Value = 1;
            f.CloseDomain();
            f.InitializeDomain();
            try {
                i.Value.ToString();
                Assert.Fail("Failed to throw a ConversationUnavailableException.");
            }
            catch (ConversationUnavailableException) {}
            f.CloseDomain();
            f.InitializeDomain(gid);
            Assert.AreEqual(1, i.Value);
            Assert.AreEqual("sometext", s.Value);
            f.CurrentConversation.FinishSpan();
            f.CloseDomain();
        }
    }
}