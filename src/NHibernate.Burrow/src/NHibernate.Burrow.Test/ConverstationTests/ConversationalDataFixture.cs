using System;
using System.Collections.Specialized;
using NHibernate.Burrow;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests {
    [TestFixture]
    public class ConversationalDataFixture  {
        readonly BurrowFramework f = new BurrowFramework();
        [Test]
        public void ConversationalDataPersistsTest() {
           
            f.InitWorkSpace();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Normal);
            ConversationalData<string> s = new ConversationalData<string>(ConversationalDataMode.Normal, "sometext");
            Assert.AreEqual(0, i.Value);
            Assert.AreEqual("sometext", s.Value);
            i.Value = 1;
            s.Value = null;
            Assert.AreEqual(1, i.Value);
            Assert.IsNull(s.Value);
            f.CurrentConversation.SpanWithPostBacks();
            Guid cid = f.CurrentConversation.Id;

            f.CloseWorkSpace();
            f.InitWorkSpace(cid);
               
            Assert.AreEqual(1, i.Value);
            Assert.IsNull(s.Value);
            f.CurrentConversation.FinishSpan();
            f.CloseWorkSpace();
            
        }

        [Test]
        public void ConversationalDataSpanBahaviorTest()
        {
            f.InitWorkSpace();
            ConversationalData<int> single = new ConversationalData<int>(ConversationalDataMode.Normal, 1);
            ConversationalData<int> singleTemp = new ConversationalData<int>(ConversationalDataMode.OutOfConversationSafe, 1);
            
            f.CurrentConversation.SpanWithPostBacks();
            Guid cid1 = f.CurrentConversation.Id;

            f.CloseWorkSpace();
            f.InitWorkSpace();
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            f.CloseWorkSpace();
            f.InitWorkSpace();
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            singleTemp.Value = 2;
            f.CurrentConversation.SpanWithPostBacks();
            Guid cid2 = f.CurrentConversation.Id;
            Assert.AreNotEqual(cid2, cid1);
            f.CloseWorkSpace();
            f.InitWorkSpace(cid1);
            Assert.AreEqual(1,single.Value);
            Assert.AreEqual(0, singleTemp.Value);
            f.CurrentConversation.FinishSpan();
            f.CloseWorkSpace();
            f.InitWorkSpace(cid2);
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            f.CurrentConversation.FinishSpan();
            f.CloseWorkSpace();
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
            f.InitWorkSpace();
            f.CurrentConversation.SpanWithPostBacks();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Normal);
            ConversationalData<string> s = new ConversationalData<string>(ConversationalDataMode.Normal, "sometext");
            Guid gid = f.CurrentConversation.Id;
            i.Value = 1;
            f.CloseWorkSpace();
            f.InitWorkSpace();
            try {
                i.Value.ToString();
                Assert.Fail("Failed to throw a ConversationUnavailableException.");
            }
            catch (ConversationUnavailableException) {}
            f.CloseWorkSpace();
            f.InitWorkSpace(gid);
            Assert.AreEqual(1, i.Value);
            Assert.AreEqual("sometext", s.Value);
            f.CurrentConversation.FinishSpan();
            f.CloseWorkSpace();
        }
    }
}