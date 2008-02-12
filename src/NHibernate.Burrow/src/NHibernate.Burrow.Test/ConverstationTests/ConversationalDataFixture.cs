using System;
using System.Collections.Specialized;
using NHibernate.Burrow;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.TestUtil;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests {
    [TestFixture]
    public class ConversationalDataFixture : TestBase {
        [Test]
        public void ConversationalDataTest() {
            Conversation.StartNew();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Single);
            Assert.AreEqual(0, i.Value);
            i.Value = 1;
            Assert.AreEqual(1, i.Value);
            Conversation.Current.AddToPool(OverspanMode.Post);
            Guid cid = Conversation.Current.Id;

            Conversation.StartNew();
            try {
                i.Value.ToString();
                Assert.Fail("Failed to throw a ConversationUnavailableException.");
            }
            catch (ConversationUnavailableException) {}

            Conversation.Current.CommitAndClose();
            Conversation.RetrieveCurrent(cid);
            Assert.AreEqual(1, i.Value);
            Conversation.Current.CommitAndClose();
        }

        [Test]
        public void DomainContextOverSpanTest() {
            Facade.StarLongConversation();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Single);
            Guid gid = Conversation.Current.Id;
            i.Value = 1;
            Facade.CloseDomain();
            Facade.InitializeDomain();
            try {
                i.Value.ToString();
                Assert.Fail("Failed to throw a ConversationUnavailableException.");
            }
            catch (ConversationUnavailableException) {}
            DomainContext.Current.Close();
            NameValueCollection mockRequest = new NameValueCollection();
            mockRequest.Add(DomainContext.ConversationIdKeyName, gid.ToString());
            Facade.InitializeDomain(mockRequest);
            Assert.AreEqual(1, i.Value);
        }
    }
}