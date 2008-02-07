using System;
using System.Collections.Specialized;
using NHibernate.Burrow.NHDomain;
using NHibernate.Burrow.NHDomain.Exceptions;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests {
    [TestFixture]
    public class ConversationalDataFixture : NHTestBase
    {
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
            DomainContext.Current.StarLongConversation();
            ConversationalData<int> i = new ConversationalData<int>(ConversationalDataMode.Single);
            Guid gid = Conversation.Current.Id;
            i.Value = 1;
            DomainContext.Current.Close();
            DomainContext.Initialize();
            try {
                i.Value.ToString();
                Assert.Fail("Failed to throw a ConversationUnavailableException.");
            }
            catch (ConversationUnavailableException) {}
            DomainContext.Current.Close();
            NameValueCollection mockRequest = new NameValueCollection();
            mockRequest.Add(DomainContext.ConversationIdKeyName, gid.ToString());
            DomainContext.Initialize(mockRequest);
            Assert.AreEqual(1, i.Value);
        }
    }
}