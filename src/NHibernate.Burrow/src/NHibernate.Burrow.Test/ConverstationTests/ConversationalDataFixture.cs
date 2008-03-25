using System;
using System.Collections.Specialized;
using NHibernate.Burrow;
using NHibernate.Burrow.Exceptions;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests {
    [TestFixture]
    public class ConversationalDataFixture  {
        [Test]
        public void ConversationalDataPersistsTest() {
            Conversation.StartNew();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Single);
            ConversationalData<string> s = new ConversationalData<string>(ConversationalDataMode.Single, "sometext");
            Assert.AreEqual(0, i.Value);
            Assert.AreEqual("sometext", s.Value);
            i.Value = 1;
            s.Value = null;
            Assert.AreEqual(1, i.Value);
            Assert.IsNull(s.Value);
            
            Conversation.Current.AddToPool(OverspanStrategy.Post);
            Guid cid = Conversation.Current.Id;

            Conversation.StartNew();
            Conversation.Current.CommitAndClose();
            Conversation.RetrieveCurrent(cid);

            Assert.AreEqual(1, i.Value);
            Assert.IsNull(s.Value);

            Conversation.Current.CommitAndClose();
        }

        [Test]
        public void ConversationalDataSpanBahaviorTest()
        {
            Conversation.StartNew();
            ConversationalData<int> single = new ConversationalData<int>(ConversationalDataMode.Single, 1);
            ConversationalData<int> singleTemp = new ConversationalData<int>(ConversationalDataMode.SingleTemp, 1);
            ConversationalData<int> overspan = new ConversationalData<int>(ConversationalDataMode.Overspan, 1);
            
            Conversation.Current.AddToPool(OverspanStrategy.Post);
            Guid cid1 = Conversation.Current.Id;

            Conversation.StartNew();
            Conversation.Current.CommitAndClose();
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            Assert.AreEqual(0, overspan.Value);
            Conversation.StartNew();
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            Assert.AreEqual(0, overspan.Value);
            singleTemp.Value = 2;
            overspan.Value = 2;
            Conversation.Current.AddToPool(OverspanStrategy.Post);
            Guid cid2 = Conversation.Current.Id;
            Assert.AreNotEqual(cid2, cid1);
             
            Conversation.RetrieveCurrent(cid1);
            Assert.AreEqual(1,single.Value);
            Assert.AreEqual(0, singleTemp.Value);
            Assert.AreEqual(1, overspan.Value);
            Conversation.Current.CommitAndClose();
            
            Conversation.RetrieveCurrent(cid2);
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            Assert.AreEqual(2, overspan.Value);
            Conversation.Current.CommitAndClose();
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
            Facade.InitializeDomain();
            Facade.StarLongConversation();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Single);
            ConversationalData<string> s = new ConversationalData<string>(ConversationalDataMode.Single, "sometext");
            Guid gid = Conversation.Current.Id;
            i.Value = 1;
            Facade.CloseDomain();
            Facade.InitializeDomain();
            try {
                i.Value.ToString();
                Assert.Fail("Failed to throw a ConversationUnavailableException.");
            }
            catch (ConversationUnavailableException) {}
            Facade.CloseDomain();
            NameValueCollection mockRequest = new NameValueCollection();
            mockRequest.Add(DomainContext.ConversationIdKeyName, gid.ToString());
            Facade.InitializeDomain(mockRequest);
            Assert.AreEqual(1, i.Value);
            Assert.AreEqual("sometext", s.Value);
            Facade.FinishOverSpanConversation();
        }
    }
}