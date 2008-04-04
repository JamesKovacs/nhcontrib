using System;
using System.Collections.Specialized;
using NHibernate.Burrow;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests {
    [TestFixture]
    public class ConversationalDataFixture  {
        [Test]
        public void ConversationalDataPersistsTest() {
            ConversationImpl.StartNew();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Single);
            ConversationalData<string> s = new ConversationalData<string>(ConversationalDataMode.Single, "sometext");
            Assert.AreEqual(0, i.Value);
            Assert.AreEqual("sometext", s.Value);
            i.Value = 1;
            s.Value = null;
            Assert.AreEqual(1, i.Value);
            Assert.IsNull(s.Value);
            
            ConversationImpl.Current.AddToPool(SpanStrategy.Post);
            Guid cid = ConversationImpl.Current.Id;

            ConversationImpl.StartNew();
            ConversationImpl.Current.CommitAndClose();
            ConversationImpl.RetrieveCurrent(cid);

            Assert.AreEqual(1, i.Value);
            Assert.IsNull(s.Value);

            ConversationImpl.Current.CommitAndClose();
        }

        [Test]
        public void ConversationalDataSpanBahaviorTest()
        {
            ConversationImpl.StartNew();
            ConversationalData<int> single = new ConversationalData<int>(ConversationalDataMode.Single, 1);
            ConversationalData<int> singleTemp = new ConversationalData<int>(ConversationalDataMode.SingleTemp, 1);
            ConversationalData<int> overspan = new ConversationalData<int>(ConversationalDataMode.Overspan, 1);
            
            ConversationImpl.Current.AddToPool(SpanStrategy.Post);
            Guid cid1 = ConversationImpl.Current.Id;

            ConversationImpl.StartNew();
            ConversationImpl.Current.CommitAndClose();
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            Assert.AreEqual(0, overspan.Value);
            ConversationImpl.StartNew();
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            Assert.AreEqual(0, overspan.Value);
            singleTemp.Value = 2;
            overspan.Value = 2;
            ConversationImpl.Current.AddToPool(SpanStrategy.Post);
            Guid cid2 = ConversationImpl.Current.Id;
            Assert.AreNotEqual(cid2, cid1);
             
            ConversationImpl.RetrieveCurrent(cid1);
            Assert.AreEqual(1,single.Value);
            Assert.AreEqual(0, singleTemp.Value);
            Assert.AreEqual(1, overspan.Value);
            ConversationImpl.Current.CommitAndClose();
            
            ConversationImpl.RetrieveCurrent(cid2);
            ExpectConversationUnavailableException(single);
            Assert.AreEqual(0, singleTemp.Value);
            Assert.AreEqual(2, overspan.Value);
            ConversationImpl.Current.CommitAndClose();
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
            Facade.CurrentConversation.SpanWithPostBacks();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Single);
            ConversationalData<string> s = new ConversationalData<string>(ConversationalDataMode.Single, "sometext");
            Guid gid = ConversationImpl.Current.Id;
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
            mockRequest.Add(ConversationImpl.ConversationIdKeyName, gid.ToString());
            Facade.InitializeDomain(mockRequest);
            Assert.AreEqual(1, i.Value);
            Assert.AreEqual("sometext", s.Value);
            Facade.CurrentConversation.FinishSpan();
        }
    }
}