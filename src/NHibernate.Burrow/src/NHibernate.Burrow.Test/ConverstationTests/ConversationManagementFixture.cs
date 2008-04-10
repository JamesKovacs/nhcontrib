using System;
using NHibernate.Burrow.Exceptions;
using NHibernate.Burrow.Impl;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.Test.UtilTests.DAO;
using NHibernate.Burrow.TestUtil;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests
{
    [TestFixture]
    public class ConversationManagementFixture : TestBase
    {
        private Facade f = new Facade();

        protected override bool CleanAndCreateSchema
        {
            get
            {
                return true;
            }
        }

        
        
        [Test]
        public void ManualRollbackTest()
        {
            MockEntities.MockEntity me = new MockEntity();
            me.Save();
            Assert.IsNotNull(new MockEntityDAO().Get(me.Id));
           
            new Facade().CurrentConversation.GiveUp();
            new Facade().CloseDomain();
            new Facade().InitializeDomain();
            Assert.IsNull( new MockEntityDAO().Get(me.Id));
        }

        [Test]
        public void ConversationInPoolTest()
        {
            f.CurrentConversation.SpanWithPostBacks();
            Guid cid1 = f.CurrentConversation.Id;
            Assert.AreEqual(1, f.BurrowEnvironment.SpanningConversations);
            f.CloseDomain();
            f.InitializeDomain();
            Assert.AreEqual(1, f.BurrowEnvironment.SpanningConversations);
            f.CurrentConversation.SpanWithPostBacks();
            Guid cid2 = f.CurrentConversation.Id;
            Assert.AreNotEqual(cid2, cid1);
            Assert.AreEqual(2, f.BurrowEnvironment.SpanningConversations);
            f.CloseDomain();
            f.InitializeDomain(cid1);
            Assert.AreEqual(cid1, f.CurrentConversation.Id);
            f.CurrentConversation.FinishSpan();
            f.CloseDomain();
            Assert.IsNull(f.CurrentConversation);
            f.InitializeDomain(cid2);
            Assert.AreEqual(cid2, f.CurrentConversation.Id);
            f.CurrentConversation.FinishSpan();
            f.CloseDomain();
            Assert.IsNull(f.CurrentConversation);
        }
    }
}