using System;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;
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
                ConversationImpl.Current.AddToPool(SpanStrategy.DoNotSpan);
                Assert.Fail("Shoult not reach here");
            }
            catch (ArgumentException) {}
        }

        [Test]
        public void CannotCommitAfterCancelTest()
        {
            ConversationImpl.Current.GiveUp();

            try
            {
                ConversationImpl.Current.CommitAndClose();
                Assert.Fail("Shoult not reach here");
            }
            catch (ConversationUnavailableException) {}

            try
            {
                ConversationImpl.Current.ForceCommitChange();
                Assert.Fail("Shoult not reach here");
            }
            catch (ConversationUnavailableException) {}
        }

        [Test]
        public void PostConversationOverspanStrategyTest()
        {
            Facade.CurrentConversation.SpanWithPostBacks();
            Assert.AreEqual(SpanStrategy.Post, ConversationImpl.Current.SpanStrategy);
             Facade.CurrentConversation.GiveUp();
        }

        [Test]
        public void SessionConversationOverspanStrategyTest()
        {
            Facade.CurrentConversation.SpanWithHttpSession();
            Assert.AreEqual(SpanStrategy.Cookie, ConversationImpl.Current.SpanStrategy);
             Facade.CurrentConversation.GiveUp();
        }

        [Test]
        public void TempConversationOverspanStrategyTest()
        {
            Assert.AreEqual(SpanStrategy.DoNotSpan, ConversationImpl.Current.SpanStrategy);
        }

        [Test]
        public void ManualRollbackTest()
        {
            MockEntities.MockEntity me = new MockEntity();
            me.Save();
            Assert.IsNotNull(new MockEntities.MockDAO().FindById(me.Id));
           
            Facade.CurrentConversation.GiveUp();
            Facade.CloseDomain();
            Facade.InitializeDomain();
            Assert.IsNull( new MockEntities.MockDAO().FindById(me.Id));
        }

        [Test]
        public void ConversationInPoolTest()
        {
            ConversationImpl.StartNew();
            ConversationImpl.Current.AddToPool(SpanStrategy.Post);
            Guid cid1 = ConversationImpl.Current.Id;
            Assert.AreEqual(1, ConversationImpl.NumOfOutStandingLongConversations);
            ConversationImpl.StartNew();  
            Assert.AreEqual(1, ConversationImpl.NumOfOutStandingLongConversations);
            ConversationImpl.Current.AddToPool(SpanStrategy.Post);
            Guid cid2 = ConversationImpl.Current.Id;
            Assert.AreNotEqual(cid2, cid1);
            Assert.AreEqual(2, ConversationImpl.NumOfOutStandingLongConversations);

            ConversationImpl.RetrieveCurrent(cid1);
            Assert.AreEqual(cid1, ConversationImpl.Current.Id);
            ConversationImpl.Current.CommitAndClose();
            Assert.IsNull(ConversationImpl.Current);
            ConversationImpl.RetrieveCurrent(cid2);
            Assert.AreEqual(cid2, ConversationImpl.Current.Id);
            ConversationImpl.Current.CommitAndClose();
            Assert.IsNull(ConversationImpl.Current);
        }
    }
}