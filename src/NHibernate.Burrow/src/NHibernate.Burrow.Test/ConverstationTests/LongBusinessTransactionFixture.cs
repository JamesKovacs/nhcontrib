using System;
using NHibernate.Burrow.TestUtil;
using NUnit.Framework;

namespace NHibernate.Burrow.Test.ConverstationTests {
	[TestFixture]
	public class LongBusinessTransactionFixture : TestBase {
		private BurrowFramework f = new BurrowFramework();

		[Test]
		public void FlushModeTest() {
			f.CurrentConversation.SpanWithPostBacks(TransactionStrategy.BusinessTransaction);

			Guid cid1 = f.CurrentConversation.Id;
			Assert.AreEqual(FlushMode.Never, f.GetSession().FlushMode);
			f.CloseWorkSpace();
			f.InitWorkSpace(cid1);
			Assert.AreEqual(FlushMode.Never, f.GetSession().FlushMode);
			f.CurrentConversation.FinishSpan();
			Assert.AreEqual(FlushMode.Auto, f.GetSession().FlushMode);

		}
	}
}