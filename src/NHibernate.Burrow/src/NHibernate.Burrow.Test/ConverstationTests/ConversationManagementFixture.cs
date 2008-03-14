using System;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.TestUtil;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests
{
    [TestFixture]
    public class ConversationManagementFixture : TestBase
    {
        [Test]
        public void CannotAddToPoolWithInvalidOverspanStrategy()
        {
            try
            {
                Conversation.Current.AddToPool(OverspanStrategy.DoNotSpan);
                Assert.Fail("Shoult not reach here");
            }
            catch (ArgumentException) {}
        }

        [Test]
        public void CannotCommitAfterCancelTest()
        {
            Conversation.Current.Cancel();

            try
            {
                Conversation.Current.CommitAndClose();
                Assert.Fail("Shoult not reach here");
            }
            catch (ConversationUnavailableException) {}

            try
            {
                Conversation.Current.CommitCurrentChange();
                Assert.Fail("Shoult not reach here");
            }
            catch (ConversationUnavailableException) {}
        }

        [Test]
        public void PostConversationOverspanStrategyTest()
        {
            Facade.StarLongConversation();
            Assert.AreEqual(OverspanStrategy.Post, Conversation.Current.OverspanStrategy);
            Facade.CancelConversation();
        }

        [Test]
        public void SessionConversationOverspanStrategyTest()
        {
            Facade.StarSessionConversation();
            Assert.AreEqual(OverspanStrategy.Cookie, Conversation.Current.OverspanStrategy);
            Facade.CancelConversation();
        }

        [Test]
        public void TempConversationOverspanStrategyTest()
        {
            Assert.AreEqual(OverspanStrategy.DoNotSpan, Conversation.Current.OverspanStrategy);
        }

        [Test]
        public void ManualRollbackTest()
        {
            MockEntities.MockEntity me = new MockEntity();
            me.Save();
            Assert.IsNotNull(new MockEntities.MockDAO().FindById(me.Id));
            Facade.CancelConversation();
            Facade.CloseDomain();
            Facade.InitializeDomain();
            Assert.IsNull( new MockEntities.MockDAO().FindById(me.Id));
        }

        [Test]
        public void ConversationInPoolTest()
        {
            Conversation.StartNew();
            Conversation.Current.AddToPool(OverspanStrategy.Post);
            Guid cid1 = Conversation.Current.Id;
            Assert.AreEqual(1, Conversation.NumOfOutStandingLongConversations);
            Conversation.StartNew();  
            Assert.AreEqual(1, Conversation.NumOfOutStandingLongConversations);
            Conversation.Current.AddToPool(OverspanStrategy.Post);
            Guid cid2 = Conversation.Current.Id;
            Assert.AreNotEqual(cid2, cid1);
            Assert.AreEqual(2, Conversation.NumOfOutStandingLongConversations);

            Conversation.RetrieveCurrent(cid1);
            Assert.AreEqual(cid1, Conversation.Current.Id);
            Conversation.Current.CommitAndClose();
            Assert.IsNull(Conversation.Current);
            Conversation.RetrieveCurrent(cid2);
            Assert.AreEqual(cid2, Conversation.Current.Id);
            Conversation.Current.CommitAndClose();
            Assert.IsNull(Conversation.Current);
        }
    }
}