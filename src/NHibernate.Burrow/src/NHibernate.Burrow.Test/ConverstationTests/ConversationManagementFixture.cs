using System;
using NHibernate.Burrow.Test.MockEntities;
using NHibernate.Burrow.TestUtil;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests
{
    [TestFixture]
    public class ConversationManagementFixture : TestBase
    {
        private BurrowFramework f = new BurrowFramework();

        protected override bool CleanAndCreateSchema
        {
            get { return true; }
        }

        [Test]
        public void ConversationInPoolTest()
        {
            f.CurrentConversation.SpanWithPostBacks();
            Guid cid1 = f.CurrentConversation.Id;
            Assert.AreEqual(1, f.BurrowEnvironment.SpanningConversations);
            f.CloseWorkSpace();
            f.InitWorkSpace();
            Assert.AreEqual(1, f.BurrowEnvironment.SpanningConversations);
            f.CurrentConversation.SpanWithPostBacks();
            Guid cid2 = f.CurrentConversation.Id;
            Assert.AreNotEqual(cid2, cid1);
            Assert.AreEqual(2, f.BurrowEnvironment.SpanningConversations);
            f.CloseWorkSpace();
            f.InitWorkSpace(cid1);
            Assert.AreEqual(cid1, f.CurrentConversation.Id);
            f.CurrentConversation.FinishSpan();
            f.CloseWorkSpace();
            Assert.IsNull(f.CurrentConversation);
            f.InitWorkSpace(cid2);
            Assert.AreEqual(cid2, f.CurrentConversation.Id);
            f.CurrentConversation.FinishSpan();
            f.CloseWorkSpace();
            Assert.IsNull(f.CurrentConversation);
        }

        [Test]
        public void ManualRollbackTest()
        {
            MockEntity me = new MockEntity();
            me.Save();
            Assert.IsNotNull(new MockEntityDAO().Get(me.Id));

            new BurrowFramework().CurrentConversation.GiveUp();
            new BurrowFramework().CloseWorkSpace();
            new BurrowFramework().InitWorkSpace();
            Assert.IsNull(new MockEntityDAO().Get(me.Id));
        }
    }
}